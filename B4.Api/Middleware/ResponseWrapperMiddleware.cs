using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace B4.Api.Middleware
{
    
    public sealed class ResponseWrapperMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            var originalBodyStream = context.Response.Body;
            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            int coderror = 0;
            string mensajeError = "";
            string actionName = "";

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                coderror = -1;
                mensajeError = ex.Message;

                if (!context.Response.HasStarted)
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            finally
            {
                stopwatch.Stop();
            }

            if (string.IsNullOrEmpty(actionName))
            {
                var method = context.Request.Method;
                if (method == HttpMethods.Get) actionName = "GetActualsByPlantaEjercicioEpigrafe";
                else if (method == HttpMethods.Post) actionName = "PostActualsByPlantaEjercicioMesTipo";
                else actionName = $"{method}Actuals";
            }

            memoryStream.Position = 0;
            var originalResponseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            context.Response.Body = originalBodyStream;

            if (string.IsNullOrEmpty(originalResponseBody) && coderror != 0)
                originalResponseBody = "{}";

            if (context.Response.StatusCode >= 400)
            {
                coderror = coderror == 0 ? context.Response.StatusCode : coderror;

                if (string.IsNullOrEmpty(mensajeError))
                    mensajeError = !string.IsNullOrEmpty(originalResponseBody) ? originalResponseBody : "La ruta o parámetro enviado no es válido";
            }

            int count = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(originalResponseBody) && originalResponseBody.Trim() != "{}")
                {
                    using var doc = JsonDocument.Parse(originalResponseBody);

                    if (doc.RootElement.TryGetProperty("detail", out var detail) &&
                        detail.ValueKind == JsonValueKind.Array)
                        count = detail.GetArrayLength();
                    else if (doc.RootElement.ValueKind == JsonValueKind.Array)
                        count = doc.RootElement.GetArrayLength();
                    else if (doc.RootElement.ValueKind != JsonValueKind.Undefined)
                        count = 1;
                }
            }
            catch
            {
                // si no es JSON, count se queda a 0
            }

            // Data seguro: si NO es JSON, lo guardamos como texto en { raw = "..." }
            object? dataObj = null;

            if (!string.IsNullOrWhiteSpace(originalResponseBody))
            {
                var trimmed = originalResponseBody.Trim();

                if (trimmed != "{}")
                {
                    var looksJson =
                        (trimmed.StartsWith("{") && trimmed.EndsWith("}")) ||
                        (trimmed.StartsWith("[") && trimmed.EndsWith("]"));

                    if (looksJson)
                    {
                        try
                        {
                            dataObj = JsonDocument.Parse(trimmed).RootElement.Clone();
                        }
                        catch
                        {
                            dataObj = new { raw = originalResponseBody };
                        }
                    }
                    else
                    {
                        dataObj = new { raw = originalResponseBody };
                    }
                }
            }

            var wrapper = new
            {
                coderror = coderror,
                action = actionName,
                msg = mensajeError ?? "",
                ts = DateTime.UtcNow.ToString("o"),
                exectimems = stopwatch.ElapsedMilliseconds,
                count = count,
                data = dataObj
            };

            context.Response.ContentType = "application/json; charset=UTF-8";
            await context.Response.WriteAsync(JsonSerializer.Serialize(wrapper), Encoding.UTF8);
        }
    }
}
