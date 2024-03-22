using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Db;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _eventPublisher;

    public ApplicationDbContext(DbContextOptions options, IPublisher eventPublisher) 
        :base(options)
    {
        _eventPublisher = eventPublisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        
            await PublishDomainEventsAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConcurrencyException("Concurrency update occured", exception);
        }
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var events = base.ChangeTracker.Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(x => x.GetDomainEvents());

        var publishEventTasks = events.Select(x => _eventPublisher.Publish(x, cancellationToken));

        await Task.WhenAll(publishEventTasks);
    }
}