using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetCore31Api.StartupFolder.SwaggerOperationFilters
{
    public class AuthorizationOperationFilter : IOperationFilter
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            var methodAttributes = context.MethodInfo.GetCustomAttributes(true);
            var classAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);

            var hasAuthorize = classAttributes.Any(attr => attr.GetType() == typeof(AuthorizeAttribute));
            var isAnonymous = methodAttributes.Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));
            var isAuthenticated = methodAttributes.Any(attr => attr.GetType() == typeof(AuthorizeAttribute));

            if (isAnonymous)
                hasAuthorize = false;
            else if (isAuthenticated)
                hasAuthorize = true;

            if (hasAuthorize)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            }
        }
    }
}
