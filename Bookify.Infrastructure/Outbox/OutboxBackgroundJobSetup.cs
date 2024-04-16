using Microsoft.Extensions.Options;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

internal sealed class OutboxBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions;

    public OutboxBackgroundJobSetup(IOptions<OutboxOptions> options)
    {
        _outboxOptions = options.Value;
    }
    
    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(OutboxBackgroundJob);

        options
            .AddJob<OutboxBackgroundJob>(builder => builder.WithIdentity(jobName))
            .AddTrigger(triggerBuilder =>
                triggerBuilder.ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds)
                            .RepeatForever())
            );
    }
}