using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Infra.Data.Context;
using Infra.Data.Interfaces.Core;
using Infra.Data.Impl.Core;
using Infra.Data.Impl.Repositories;
using Infra.Data.Interfaces.Repositories;

namespace Infra.Data.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddApplicationDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddDbContext<ApplicationDbContext>(optionsAction, ServiceLifetime.Scoped);
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IApplicationCredentialRepository, ApplicationCredentialRepository>();
        }
    }
}
