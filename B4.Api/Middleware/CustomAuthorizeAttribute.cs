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
                // Comprobamos si el atributo CustomAuthorize especifica un permiso a validar
                if (!string.IsNullOrEmpty(Permission))
                {
                    // Convertimos la identidad del usuario a ClaimsIdentity
                    // Esto permite leer y añadir claims dinámicos
                    var identity = user.Identity as ClaimsIdentity;

                    // Validación defensiva: si no se puede obtener la identidad, lanzamos excepción
                    if (identity == null)
                    {
                        throw new InvalidOperationException("No se pudo obtener la identidad del usuario.");
                    }

                    // Decidimos el valor del permiso dinámico
                    // Aquí se puede usar lógica propia, DB, rol, query, etc.
                    bool canEdit = true;

                    // Añadimos el claim dinámico en memoria si no existe ya
                    // Esto no modifica el token JWT, solo para esta request
                    if (!identity.HasClaim(c => c.Type == Permission))
                    {
                        identity.AddClaim(new Claim(Permission, canEdit.ToString().ToLower()));
                    }

                    // Validamos que el usuario tiene el claim con valor "true"
                    // Type = Permission (ej: "can_edit")
                    // Value = "true"
                    bool hasClaim = user.HasClaim(c =>
                        c.Type == Permission && c.Value == "true");

                    // Si no tiene el permiso requerido, devolvemos 403 Forbidden
                    // Esto corta la ejecución del endpoint
                    if (!hasClaim)
                    {
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
