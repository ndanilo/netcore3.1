using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetCore31Api.StartupFolder.SwaggerOperationFilters
{
    public class FileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //if (context.ApiDescription.ParameterDescriptions.Any(x => x.Type == typeof(IFormFile)))
            //{
            //    operation.Consumes.Add("application/form-data");

            //    var existingParameterName = context.ApiDescription.ParameterDescriptions.FirstOrDefault(x => x.Type == typeof(IFormFile)).Name;
            //    var existingParameter = operation.Parameters.FirstOrDefault(x => x.Name == existingParameterName);
            //    //creates a new parameter type for the file, based on the previous one
            //    var newParameter = new NonBodyParameter()
            //    {
            //        Name = existingParameter.Name,
            //        Type = "file",
            //        Description = existingParameter.Description,
            //        Required = existingParameter.Required,
            //        In = "formData"
            //    };

            //    //insert the new parameter without changing the order
            //    int position = operation.Parameters.IndexOf(existingParameter);
            //    operation.Parameters.RemoveAt(position);
            //    operation.Parameters.Insert(position, newParameter);

            //    //foreach (var parameter in operation.Parameters)
            //    //{
            //    //    parameter.In = "";
            //    //}
            //}
            //else
            //{
            //    var addFileUploadButton = context.MethodInfo.GetCustomAttributes(typeof(AddSwaggerFileUploadButtonAttribute), false).Any();
            //    if (addFileUploadButton)
            //    {
            //        operation.Parameters.Add(new OpenApiParameter
            //        {
            //            Name = "file",
            //            In = "formData",
            //            Description = "Upload File",
            //            Required = true,
            //            Type = "file"
            //        });
            //        operation.Consumes.Add("multipart/form-data");
            //    }
            //}
        }
    }
}
