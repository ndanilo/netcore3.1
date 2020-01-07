//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.JsonPatch.Operations;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.Swagger;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace NetCore31Api.Middlewares
//{
//    /// <summary>
//    /// middleware to intercept errors at application
//    /// </summary>
//    public class AuthResponsesOperationFilter : IOperationFilter
//    {
//        public void Apply(Operation operation, OperationFilterContext context)
//        {
//            var controllerScopes = context.ApiDescription.ControllerAttributes()
//                .OfType<AuthorizeAttribute>()
//                .Select(attr => attr.Policy);

//            var actionRequiredScopes = context.ApiDescription.ActionAttributes()
//                .OfType<AuthorizeAttribute>()
//                .Select(attr => attr.Policy);

//            var actionNonRequiredScopes = context.ApiDescription.ActionAttributes()
//                .OfType<AllowAnonymousAttribute>();

//            var requiredScopes = controllerScopes.Union(actionRequiredScopes).Distinct();
//            var nonRequiredScopes = actionNonRequiredScopes;

//            if (requiredScopes.Any() && !nonRequiredScopes.Any())
//            {
//                if (!operation.Responses.Keys.Contains("401"))
//                    operation.Responses.Add("401", new Response { Description = "Unauthorized" });
//                if (!operation.Responses.Keys.Contains("403"))
//                    operation.Responses.Add("403", new Response { Description = "Forbidden" });

//                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
//                operation.Security.Add(new Dictionary<string, IEnumerable<string>>
//                {
//                    { "Bearer", requiredScopes }
//                });
//            }
//        }
//    }
//}
