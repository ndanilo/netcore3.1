using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.DTO.Exceptions;
using Services.Interfaces;

namespace NetCore31Api.Middlewares
{
    public class ApplicationAuthorizationFilter : Attribute, IAsyncResourceFilter, IFilterFactory
    {
        public bool IsReusable => false;
        private ICredentialService _credentialService;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = new ApplicationAuthorizationFilter();
            filter._credentialService = (ICredentialService)serviceProvider.GetService(typeof(ICredentialService));
            return filter;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;
            Microsoft.Extensions.Primitives.StringValues value;

            if (!headers.TryGetValue("X-ApiKey", out value))
                throw new NotAuthorizedException("O header X-ApiKey é obrigatório");

            string apiKey = value.ToString();
            bool isAuthorized = await _credentialService.IsApplicationAuthorized(apiKey);

            if (!isAuthorized)
                throw new NotAuthorizedException("ApiKey inválida");

            await next();
        }
    }
}
