using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NetCore31Api.Middlewares;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetCore31Api.StartupFolder.SwaggerOperationFilters
{
    public class ApiKeyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodAttributes = context.MethodInfo.GetCustomAttributes(true);
            var classAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);
            if (methodAttributes.Concat(classAttributes).Any(attr => attr.GetType() == typeof(ApplicationAuthorizationFilter)))
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-ApiKey",
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    Required = false
                });
            }
        }
    }
}
