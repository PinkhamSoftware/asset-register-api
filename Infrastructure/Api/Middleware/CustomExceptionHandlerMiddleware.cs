//using System;
//using System.Threading.Tasks;
//using Infrastructure.Api.Exceptions;
//using Infrastructure.Api.Response;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//
//namespace Infrastructure.Api.Middleware
//{
//    /// <summary>
//    /// Middleware to handle exceptions and return them as ApiResponses with ApiErrors and correct
//    /// HttpStatus Methods
//    /// </summary>
//    public sealed class CustomExceptionHandlerMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger _logger;
//
//        public CustomExceptionHandlerMiddleware(
//            RequestDelegate next,
//            ILoggerFactory loggerFactory)
//        {
//            _next = next;
//            _logger = loggerFactory.CreateLogger<CustomExceptionHandlerMiddleware>();
//        }
//
//        /// <summary>
//        /// Intercept the request and wrap it in a TryCatch
//        /// Allows us to throw exceptions and know they will be properly handled
//        /// Centralises exception handling and removes 80-90% of exception handling code from codebase
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public async Task Invoke(HttpContext context)
//        {
//            try
//            {
//                //execute next operation in HttpRequest pipeline -> OtherMiddleware -> controller
//                await _next(context).ConfigureAwait(false);
//            }
//            catch (ApiException ex)
//            {
//                await HandleApiException(context, ex).ConfigureAwait(false);
//            }
//            catch (Exception ex)
//            {
//                await HandleException(context, ex).ConfigureAwait(false);
//            }
//        }
//
//        private async Task HandleApiException(HttpContext context, ApiException ex)
//        {
//            LogException(ex);
//            var apiResponse = new ApiResponse<object>(ex);
//            await WriteResponseToClient(context, apiResponse).ConfigureAwait(false);
//        }
//
//        private async Task HandleException(HttpContext context, Exception ex)
//        {
//            LogException(ex);
//            var apiResponse = new ApiResponse<object>(ex);
//            await WriteResponseToClient(context, apiResponse).ConfigureAwait(false);
//        }
//
//        private static async Task WriteResponseToClient(HttpContext context, ApiResponse<object> apiResponse)
//        {
//            context.Response.ContentType = "application/json";
//            var response = JsonConvert.SerializeObject(apiResponse);
//            await context.Response.WriteAsync(response, context.RequestAborted).ConfigureAwait(false);
//        }
//
//        private void LogException( Exception ex)
//        {
//            _logger.LogError(ex, $"{nameof(ex)} occurred");
//        }
//    }
//}
