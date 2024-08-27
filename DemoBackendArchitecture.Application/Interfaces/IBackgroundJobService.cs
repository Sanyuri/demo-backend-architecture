using System.Linq.Expressions;

namespace DemoBackendArchitecture.Application.Interfaces;

public interface IBackgroundJobService
{
    void EnqueueJob(Expression<Action> methodCall);
    void RecurringJob(Expression<Action> methodCall, string cronExpression, TimeZoneInfo timeZoneInfo = null, string queue = "default");
}