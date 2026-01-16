using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace B4.Api.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Obtener el usuario autenticado del contexto
            var user = context.HttpContext.User;

            // Si no está autenticado
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                // Devuelve 401 Unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }

            // Aquí se puede agregar validación de roles o claims
            // Ejemplo:
            // if (!user.IsInRole("Admin")) context.Result = new ForbidResult();
        }
    }
}
