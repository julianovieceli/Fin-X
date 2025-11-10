using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;


namespace Fin_X.Api.Swagger;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // 1. Verifica se a ação ou o controlador tem o atributo [Authorize]
        var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>().Any() ?? false;

        var methodHasAuthorize = context.MethodInfo.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>().Any();

        if (hasAuthorize || methodHasAuthorize)
        {
            // 2. Cria a referência de segurança para o esquema 'Bearer' (JWT)
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Usa o ID definido em AddSecurityDefinition
                },
                Scheme = "oauth2" // Protocolo
            };

            // 3. Aplica o requisito de segurança à operação (endpoint)
            operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, new List<string>() }
            }
        };

            // Opcional: Adicionar um alerta visual para o desenvolvedor
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        }

        // Se você precisar que o Basic Auth seja aplicado se o endpoint não for JWT,
        // você pode adicionar lógica condicional aqui. 
        // No entanto, é mais comum usar apenas um esquema de segurança principal (JWT).
    }
}