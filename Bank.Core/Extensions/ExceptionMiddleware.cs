using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank.Core.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (AuthorizationException ex)
            {
                await HandleAuthorizationExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleAuthorizationExceptionAsync(HttpContext context, AuthorizationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            Console.WriteLine($"Hata oluştu: {ex.Message}");
            var error = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Console.WriteLine($"Hata oluştu: {ex.Message}");
            var error = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = $"InnerException: {ex.InnerException.Message}"
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    }
}
