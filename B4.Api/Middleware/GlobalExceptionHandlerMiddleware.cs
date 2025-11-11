
/*
    MIDDLEWARE
    Getion de las excecpciones en la respuesta HTTP
    20251111 - pdte revision para B4
*/
namespace B4.Api.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }


        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problemDetails = new ProblemDetails
                {
                    Status = (int)StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Type = "Server Error",
                    Instance = "",
                    Detail = @"An internal server error has ocurred",
                    //Detail = ex.Message,
                };

                var json = JsonConvert.SerializeObject(problemDetails);

                context.Response.ContentType = "application/json";

                //await context.Response.WriteAsync(json);  
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }

}
