using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Models.DTO.Exceptions;
using Models.DTO.Generics;
using Newtonsoft.Json;

namespace NetCore31Api.Middlewares
{
    /// <summary>
    /// default middleware to intercept errors at application
    /// </summary>
    public class RestfulMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="next"></param>
        public RestfulMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var error = new Error();
                var statusCode = 0;

                if (ex.IsDomainException())
                {
                    statusCode = ((BasicException)ex).ERROR_CODE;
                }
                else
                    statusCode = 500;

                error.Message = ex.Message;
                error.ErrorCode = statusCode;
                //error.StackTrace = $"stacktrace: {ex.StackTrace}; source: {ex.Source}; targetsite: {ex.TargetSite}";             

                var objSerialized = JsonConvert.SerializeObject(error);

                context.Response.Clear();
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(objSerialized);
            }
        }
    }

    /// <summary>
    /// Extension method used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class RestfulMiddlewareExtensions
    {
        public static IApplicationBuilder UseRestfulMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RestfulMiddleware>();
        }
    }
}
