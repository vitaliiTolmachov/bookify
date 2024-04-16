using System.Data;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Data;
using Bookify.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class OutboxBackgroundJob : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IPublisher _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<OutboxBackgroundJob> _logger;

    public OutboxBackgroundJob(
        IDbConnectionFactory dbConnectionFactory,
        IPublisher mediator,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<OutboxBackgroundJob> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        _outboxOptions = outboxOptions.Value;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Beginning to process outbox messages");
        
        using var connection = _dbConnectionFactory.CreateDbConnection();
        using var transaction = connection.BeginTransaction();
        
        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        await ProcessOutboxMessagesAsync(connection, transaction, outboxMessages, context.CancellationToken);
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection, 
        IDbTransaction transaction)
    {
        var sql = $"""
                   SELECT id, content
                   FROM outbox_messages
                   WHERE processed_on_utc IS NULL
                   ORDER BY occurred_on_utc
                   LIMIT @BatchSize
                   FOR UPDATE
                   """;
        
        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(
            sql,
            transaction: transaction,
            param: new
        {
            _outboxOptions.BatchSize
        });

        return outboxMessages.ToList();
    }
    
    private async Task ProcessOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        IReadOnlyList<OutboxMessageResponse> outboxMessages,
        CancellationToken cancellationToken)
    {
        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    value: outboxMessage.Content,
                    settings: JsonSerializerSettings)!;

                await _mediator.Publish(domainEvent, cancellationToken);

            }//Incrementing attempt and retry to handle the same message a few times if needed can be implemented here
            catch (Exception caughtException)
            {
                _logger.LogError(
                    caughtException,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);
                
                exception = caughtException;
            }
            
            await UpdateProcessedOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }
        
        transaction.Commit();
        _logger.LogInformation("Completed processing outbox messages");
    }

    private async Task UpdateProcessedOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql = @"
        UPDATE outbox_messages
        SET processed_on_utc = @ProcessedOnUtc, error = @Error
        WHERE id = @Id";
        
        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = _dateTimeProvider.CurrentTimeUtc,
                Error = exception?.ToString()
            },
            transaction: transaction);
    }

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}