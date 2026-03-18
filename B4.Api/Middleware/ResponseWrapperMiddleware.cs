using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace B4.Api.Middleware
{
    public sealed class ResponseWrapperMiddleware : IMiddleware
    {
        /*
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Ignorar preflight OPTIONS
            if (context.Request.Method == HttpMethods.Options)
            {
                await next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            var originalBodyStream = context.Response.Body;
            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await next(context);

            stopwatch.Stop();

            memoryStream.Position = 0;
            var originalResponseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            context.Response.Body = originalBodyStream;

            // ===== SOLO RESPUESTAS OK =====

            int count = 0;

            try
            {
                if (!string.IsNullOrWhiteSpace(originalResponseBody))
                {
                    using var doc = JsonDocument.Parse(originalResponseBody);

                    if (doc.RootElement.ValueKind == JsonValueKind.Array)
                        count = doc.RootElement.GetArrayLength();
                    else
                        count = 1;
                }
            }
            catch
            {
                count = 0;
            }

            object? dataObj = null;

            if (!string.IsNullOrWhiteSpace(originalResponseBody))
            {
                try
                {
                    dataObj = JsonDocument.Parse(originalResponseBody).RootElement.Clone();
                }
                catch
                {
                    dataObj = new { raw = originalResponseBody };
                }
            }

            var wrapper = new
            {
                coderror = 0,
                action = context.Request.Method,
                msg = "",
                ts = DateTime.UtcNow.ToString("o"),
                exectimems = stopwatch.ElapsedMilliseconds,
                count = count,
                data = dataObj
            };

            // Solo escribir si la respuesta NO empezó y no es 204
            if (!context.Response.HasStarted && context.Response.StatusCode != StatusCodes.Status204NoContent)
            {
                context.Response.ContentType = "application/json; charset=UTF-8";
                await context.Response.WriteAsync(JsonSerializer.Serialize(wrapper), Encoding.UTF8);
            }
        }

        */

    }
}