using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security;
using System.Security.Claims;

namespace B4.Api.Middleware
{
    // AllowMultiple = true permite aplicar más de un CustomAuthorize si fuera necesario.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        // Rol requerido para acceder al endpoint (ej: "Admin")
        // Se valida contra ClaimTypes.Role del JWT
        public string? Role { get; set; }

        // Nombre de una Policy registrada en Program.cs
        // Ej: options.AddPolicy("AdminOnly", ...)
        public string? Policy { get; set; }

        // Claim/permisión requerida
        // Ej: "can_edit", "can_delete"
        public string? Permission { get; set; }

        // Método que se ejecuta ANTES de entrar al endpoint
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                // Usuario autenticado extraído del HttpContext
                var user = context.HttpContext.User;

                // 1️ Verificar si el usuario está autenticado
                // Si no hay identidad o no está autenticado: 401 Unauthorized
                if (!user.Identity?.IsAuthenticated ?? true)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // 2️ Validar ROL (si se especificó)
                // Usa los claims de rol incluidos en el JWT
                if (!string.IsNullOrEmpty(Role) && !user.IsInRole(Role))
                {
                    // Usuario autenticado pero sin rol adecuado: 403 Forbidden
                    context.Result = new ForbidResult();
                    return;
                }

                // 3️ Validar PERMISO / CLAIM (si se especificó)
                // Este claim puede venir del JWT o ser añadido dinámicamente en memoria
                if (!string.IsNullOrEmpty(Permission))
                {
                    var identity = user.Identity as ClaimsIdentity;

                    if (identity == null)
                    {
                        // Si algo sale mal al leer la identidad → lanzar excepción
                        throw new InvalidOperationException("No se pudo obtener la identidad del usuario.");
                    }

                    bool canEdit = true;

                    // Añadir claim dinámico en memoria si no existe
                    if (!identity.HasClaim(c => c.Type == Permission))
                    {
                        identity.AddClaim(new Claim(Permission, canEdit.ToString().ToLower()));
                    }

                    // Comprueba que exista un claim con:
                    // Type  = Permission
                    // Value = "true"
                    bool hasClaim = user.HasClaim(c =>
                        c.Type == Permission && c.Value == "true");

                    if (!hasClaim)
                    {
                        // No tiene el permiso requerido → 403 Forbidden
                        context.Result = new ForbidResult();
                        return;
                    }
                }

                // 4️ Validar POLICY (si se especificó)
                // Usa el sistema de Authorization de ASP.NET Core
                if (!string.IsNullOrEmpty(Policy))
                {
                    // Obtener el servicio de autorización desde el contenedor DI
                    var authorizationService = context.HttpContext.RequestServices
                        .GetService<IAuthorizationService>();

                    if (authorizationService == null)
                    {
                        // No se pudo obtener el servicio de autorización: lanzar excepción
                        throw new InvalidOperationException("No se pudo obtener IAuthorizationService.");
                    }

                    // Ejecutar la policy contra el usuario actual
                    var authResult = authorizationService
                        .AuthorizeAsync(user, null, Policy)
                        .GetAwaiter()
                        .GetResult();

                    if (!authResult.Succeeded)
                    {
                        // La policy no se cumple: 403 Forbidden
                        context.Result = new ForbidResult();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // 5️ Captura cualquier excepción inesperada
                throw new SecurityException($"Error en autorización: {ex.Message}", ex);
            }
        }
    }
}
