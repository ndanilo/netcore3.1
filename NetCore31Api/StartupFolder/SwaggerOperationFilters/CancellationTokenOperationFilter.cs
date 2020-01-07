using System.Linq;
using System.Threading;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetCore31Api.StartupFolder.SwaggerOperationFilters
{
    public class CancellationTokenOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            
            var excludedParameters = context.ApiDescription.ParameterDescriptions
                .Where(p => p.ParameterDescriptor?.ParameterType == typeof (CancellationToken))
                .Select(p => operation.Parameters.FirstOrDefault(operationParam => operationParam.Name == p.Name));

            foreach (var parameter in excludedParameters)
                operation.Parameters.Remove(parameter);
        }
    }
}