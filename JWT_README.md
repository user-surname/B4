Guía rápida: CustomAuthorizeAttribute

La clase CustomAuthorizeAttribute permite controlar el acceso a endpoints de ASP.NET Core mediante roles, claims y policies.

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string? Role { get; set; }
    public string? Policy { get; set; }
    public string? Permission { get; set; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Validaciones
    }
}

1 Role

Propiedad: Role

Permite restringir acceso según el rol del usuario definido en el claim ClaimTypes.Role dentro del JWT.

Ejemplo de uso:

[CustomAuthorize(Role = "Admin")]


 Comportamiento:

Si el usuario no tiene ese rol → 403 Forbidden

Si tiene el rol → acceso permitido

2 Permission (claim dinámico)

Propiedad: Permission

Permite validar un claim específico que represente un permiso (can_edit, can_view, etc.)

Ejemplo de uso:

[CustomAuthorize(Permission = "can_edit")]


 Comportamiento:

Comprueba User.HasClaim(c => c.Type == Permission && c.Value == "true")

El claim puede ser dinámico (añadido temporalmente en memoria) o venir del JWT

Si no tiene el claim → 403 Forbidden

Un claim dinámico agregado en memoria solo dura durante la request actual.
Para persistirlo, debe regenerarse el JWT con ese claim y enviarlo al cliente.

3 Policy
Propiedad: Policy

Permite validar reglas definidas en AuthorizationOptions dentro de Program.cs

Ejemplo de uso:

[CustomAuthorize(Policy = "AdminOnly")]


 Comportamiento:

Llama a IAuthorizationService.AuthorizeAsync(user, null, Policy)

Si la policy no se cumple → 403 Forbidden

Permite combinaciones complejas de roles, claims o requisitos personalizados