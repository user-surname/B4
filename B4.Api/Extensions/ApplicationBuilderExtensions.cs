using B4.Api.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Extensions;

// ============================================================
// ApplicationBuilderExtensions.cs — Pipeline de middlewares HTTP
// ============================================================

public static class ApplicationBuilderExtensions
{
    // Método de extensión sobre WebApplication que centraliza la configuración
    // del pipeline de middlewares, manteniendo Program.cs limpio y legible.
    public static WebApplication UseB4MiddlewarePipeline(this WebApplication app)
    {
        // En entorno de desarrollo se habilita Swagger UI con soporte multi-versión:
        // itera sobre todas las versiones registradas y añade un endpoint por cada una.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var provider = app.Services.GetRequiredService<Microsoft.AspNetCore.Mvc.ApiExplorer.IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            // Muestra información detallada de excepciones en el navegador (solo desarrollo)
            app.UseDeveloperExceptionPage();
        }

        // Redirige automáticamente las peticiones HTTP a HTTPS
        app.UseHttpsRedirection();

        // Habilita el enrutamiento, necesario antes de UseAuthentication/UseAuthorization
        app.UseRouting();

        // Middleware global de manejo de excepciones no controladas:
        // captura cualquier excepción que ocurra en los middlewares siguientes
        // y devuelve una respuesta JSON estructurada en lugar de una página de error genérica.
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        // Middleware inline para detectar rutas no registradas (endpoint == null).
        // Debe ir ANTES de UseAuthentication para interceptar rutas inexistentes
        // antes de intentar validar credenciales, devolviendo un 404 en formato JSON.
        app.Use(async (context, next) =>
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    coderror = 404,
                    action = context.Request.Path,
                    msg = "Ruta o parámetro enviado no es válido",
                    ts = DateTime.UtcNow,
                    exectimems = 0,
                    count = 0,
                    data = (object?)null
                });
                return; // Cortocircuita el pipeline: no continúa a los siguientes middlewares
            }

            await next(); // La ruta existe: continúa el pipeline normalmente
        });

        // Valida el token JWT en el header Authorization y establece la identidad del usuario (ClaimsPrincipal)
        app.UseAuthentication();

        // Comprueba que el usuario autenticado tiene permisos para acceder al endpoint solicitado
        app.UseAuthorization();

        // Middleware de páginas de estado: intercepta respuestas con códigos de error concretos
        // para formatearlas. Aquí se sobreescribe el 404 residual (rutas que pasan el routing
        // pero no tienen handler) con un ProblemDetails estándar de RFC 7807.
        app.UseStatusCodePages(async context =>
        {
            var response = context.HttpContext.Response;
            if (response.StatusCode == StatusCodes.Status404NotFound)
            {
                response.ContentType = "application/json";
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Route Not Found",
                    Type = "https://httpstatuses.com/404",
                    Detail = "The requested route does not exist.",
                    Instance = context.HttpContext.Request.Path
                };

                await response.WriteAsJsonAsync(problemDetails);
            }
        });

        // Aplica la política CORS "AllowAll" (cualquier origen, cabecera y método).
        // Debe ir después de UseRouting y antes de los controladores para que los
        // preflight OPTIONS sean gestionados correctamente.
        app.UseCors("AllowAll");

        return app;
    }
}