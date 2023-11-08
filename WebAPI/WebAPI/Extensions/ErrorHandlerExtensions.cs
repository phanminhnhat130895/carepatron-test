using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using System.Text.Json;

namespace WebAPI.Extensions
{
    public static class ErrorHandlerExtensions
    {
        public static void UseErrorHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature == null) return;

                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.ContentType = "application/json";

                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        BadRequestException => (int)HttpStatusCode.BadRequest,
                        OperationCanceledException => (int)HttpStatusCode.ServiceUnavailable,
                        NotFoundException => (int)HttpStatusCode.NotFound,
                        ResourceConflictException => (int)HttpStatusCode.Conflict,
                        _ => (int)HttpStatusCode.InternalServerError
                    };

                    var errorResponse = new ErrorResponse()
                    {
                        statusCode = context.Response.StatusCode,
                        message = string.Empty
                    };

                    if (contextFeature.Error is BadRequestException)
                    {
                        var error = contextFeature.Error as BadRequestException;
                        errorResponse.message = error == null ? string.Empty : string.Join(",", error.Errors);
                    }
                    else
                    {
                        errorResponse.message = contextFeature.Error.Message;
                    }

                    Log.Error("Exception: {@error}", contextFeature.Error);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                });
            });
        }
    }

    public class ErrorResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
    }
}
