using System.Linq.Expressions;
using DemoBackendArchitecture.Application.Interfaces;
using Hangfire;

namespace DemoBackendArchitecture.API.Helpers;

public class BackgroundJobService : IBackgroundJobService
{
    public void EnqueueJob(Expression<Action> methodCall)
    {
        BackgroundJob.Enqueue(methodCall);
    }

    public void RecurringJob(Expression<Action> methodCall, string cronExpression, TimeZoneInfo timeZoneInfo = null, string queue = "default")
    {
        Hangfire.RecurringJob.AddOrUpdate(methodCall, cronExpression, timeZoneInfo, queue);
    }
}