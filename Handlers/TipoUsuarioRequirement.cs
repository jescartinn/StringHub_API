using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StringHub.Handlers
{
    public class TipoUsuarioRequirement : IAuthorizationRequirement
    {
        public string[] TiposUsuarioPermitidos { get; }

        public TipoUsuarioRequirement(params string[] tiposUsuarioPermitidos)
        {
            TiposUsuarioPermitidos = tiposUsuarioPermitidos;
        }
    }

    public class TipoUsuarioAuthorizationHandler : AuthorizationHandler<TipoUsuarioRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TipoUsuarioRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "tipoUsuario"))
            {
                return Task.CompletedTask;
            }

            var tipoUsuario = context.User.FindFirst(c => c.Type == "tipoUsuario")?.Value;

            if (tipoUsuario != null && requirement.TiposUsuarioPermitidos.Contains(tipoUsuario))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}