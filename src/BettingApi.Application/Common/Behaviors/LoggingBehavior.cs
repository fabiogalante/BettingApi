using System.Diagnostics;
using BettingApi.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace BettingApi.Application.Common.Behaviors;

public class LoggingBehavior<TInput, TOutput>(ILogger<LoggingBehavior<TInput, TOutput>> logger)
    : IPipelineBehavior<TInput, TOutput>
{
    public async Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request = {Request} - Response = {Response} ", typeof(TInput).Name, typeof(TOutput).Name);
        
        var timer = new Stopwatch();
        timer.Start();
        
        var result = await next();
        
        timer.Stop();
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds",
                typeof(TInput).Name, timeTaken.Seconds);
       
        logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TInput).Name, typeof(TOutput).Name);
        
        return result;
    }
}
