using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;


namespace Fin_X.Api.Swagger;


    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for Authorize attribute on the method or controller
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                               context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                // Initialize the security requirement list
                operation.Security = new List<OpenApiSecurityRequirement>();

                // 1. Add JWT Bearer requirement
                operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer" // Matches AddSecurityDefinition ID
                        }
                    },
                    Array.Empty<string>()
                }
            });

                // 2. Add Basic Authentication requirement
                operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "BasicAuthentication" // Matches AddSecurityDefinition ID
                        }
                    },
                    Array.Empty<string>()
                }
            });

                // NOTE: If your API is configured to accept EITHER JWT OR Basic Auth, 
                // the above code is correct. If you wanted them to be a single, combined 
                // requirement (less common), the approach would be different.
            }
        }
    }
