using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StARKS.Application.Infrastructure
{
    public class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> logger;

        public LoggingBehavior(ILogger<TRequest> logger)
            => this.logger = logger;

        public async Task<TResponse> Handle(
            TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                logger.LogInformation($"StARKS.Application [Request Name: [{request}]");
                var response = await next();
                logger.LogInformation("Called handler with result {0}", response);
                return response;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error occured in StARKS.Application [Request:{request}], exception Message: [{ex.Message}]");

                throw ex;
            }
        }
    }
}
