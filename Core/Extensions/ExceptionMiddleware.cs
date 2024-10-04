using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Http;


namespace Core.Extensions
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
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //http status code u 500 dür 
            string Message;
            var type = e.GetType();


            if (type == typeof(ValidationException))
            {
                Message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (type == typeof(ApplicationException))
            {
                Message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (type == typeof(UnauthorizedAccessException))
            {
                Message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (type == typeof(SecurityException))
            {
                Message = e.Message;
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            else if (type == typeof(NotSupportedException))
            {
                Message = e.Message;
                httpContext.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            }

            else
            {
                Message = "Something went wrong. Please try again later.";
            }

            var response = JsonSerializer.Serialize(new ErrorResult(httpContext.Response.StatusCode, Message));
            await httpContext.Response.WriteAsync(response);

        }


    }
}
