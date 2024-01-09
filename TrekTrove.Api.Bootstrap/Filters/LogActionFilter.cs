using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace TrekTrove.Api.Bootstrap.Filters
{
    [ExcludeFromCodeCoverage]
    public class LogActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        private const string DEFAULT_MESSAGE_FORMAT = "ReceiveMessage::{0} | {1} Headers: {2} Route values: {3} Parameters: {4}";
        private const string ERROR_MESSAGE_FORMAT = "ReceiveMessage::{0} | {1} Headers: {2} Route values: {3} - Request unsuccessful!";
        private const string SUCCESS_MESSAGE_FORMAT = "ReceiveMessage::{0} | {1} Headers: {2} Route values: {3} - Request successful!";
        private const string TRACE_MESSAGE_FORMAT = "ReceiveMessage::{0} | {1} ResultData: {2}";
        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var headers = JsonConvert.SerializeObject(context.HttpContext.Request.Headers.ToDictionary(k => k.Key, v => v.Value.First()));
            var routeValues = JsonConvert.SerializeObject(context.RouteData.Values);
            var parameters = JsonConvert.SerializeObject(context.ActionArguments);
            var operationName = JsonConvert.SerializeObject(context.HttpContext.Request.Path.Value);

            var logMessage = string.Format(DEFAULT_MESSAGE_FORMAT, DateTime.Now, operationName, headers, routeValues, parameters);
            _logger.LogInformation(logMessage);

            await next();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var headers = JsonConvert.SerializeObject(context.HttpContext.Request.Headers.ToDictionary(k => k.Key, v => v.Value.First()));
            var routeValues = JsonConvert.SerializeObject(context.RouteData.Values);
            var operationName = JsonConvert.SerializeObject(context.HttpContext.Request.Path.Value);
            var logMessage = string.Format(ERROR_MESSAGE_FORMAT, DateTime.Now, operationName, headers, routeValues);

            if (context.HttpContext.Response.StatusCode != (int)HttpStatusCode.OK)
            {
                _logger.LogError(logMessage);
            }
            else
            {
                logMessage = string.Format(SUCCESS_MESSAGE_FORMAT, DateTime.Now, operationName, headers, routeValues);
                _logger.LogInformation(logMessage);
            }

            var resultData = JsonConvert.SerializeObject(context.Result);
            logMessage = string.Format(TRACE_MESSAGE_FORMAT, DateTime.Now, operationName, resultData);
            _logger.LogTrace(logMessage);
        }
    }
}
