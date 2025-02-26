using Microsoft.AspNetCore.Authorization;
using StringHub.Handlers;

namespace StringHub.Data
{
    public static class AuthorizationPolicies
    {
        public static void ConfigurePolicies(AuthorizationOptions options)
        {
            // Política para administradores
            options.AddPolicy("EsAdmin", policy =>
                policy.AddRequirements(new TipoUsuarioRequirement("Admin")));

            // Política para encordadores
            options.AddPolicy("EsEncordador", policy =>
                policy.AddRequirements(new TipoUsuarioRequirement("Encordador")));

            // Política para clientes
            options.AddPolicy("EsCliente", policy =>
                policy.AddRequirements(new TipoUsuarioRequirement("Cliente")));

            // Política para administradores o encordadores
            options.AddPolicy("EsAdminOEncordador", policy =>
                policy.AddRequirements(new TipoUsuarioRequirement("Admin", "Encordador")));

            // Política para acceso de staff (admin y encordadores)
            options.AddPolicy("EsStaff", policy =>
                policy.AddRequirements(new TipoUsuarioRequirement("Admin", "Encordador")));
        }
    }
}