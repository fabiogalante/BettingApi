using BettingApi.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace BettingApi.Application.Common.Behaviors;

public class LoggingBehavior<TInput, TOutput>(ILogger<LoggingBehavior<TInput, TOutput>> logger)
    : IPipelineBehavior<TInput, TOutput>
{
    public async Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {RequestName} with data: {@Request}", typeof(TInput).Name, input);
        var result = await next();
        logger.LogInformation("Handled {RequestName} successfully", typeof(TOutput).Name);
        return result;
    }
}
