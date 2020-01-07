using Microsoft.Extensions.DependencyInjection;
using Models.Entities;
using Services.Impl;
using Services.Interfaces;

namespace Services.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            #region Intergrations

            //services.AddTransient<IRekognitionService, RekognitionService>();
            //services.AddTransient<ICloudVisionService, CloudVisionService>();

            #endregion

            #region Locals

            services.AddTransient<ICredentialService, CredentialService>();

            #endregion
        }
    }
}
