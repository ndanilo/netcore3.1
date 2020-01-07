using System;
using System.Collections.Generic;
using System.IO;
using Infra.Data.Auth;
using Infra.Data.Context;
using Infra.Data.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Models.Entities;
using Models.Entities.Identity;
using NetCore31Api.Middlewares;
using NetCore31Api.StartupFolder.SwaggerOperationFilters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.EntityFrameworkCore.Design;
using Services.Extensions;

namespace NetCore31Api.StartupFolder
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string sconn = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

            sconn =
                "Server=localhost;Initial Catalog=NetCore31Poc;Persist Security Info=False;User ID=root;Password=root;MultipleActiveResultSets=False;Connection Timeout=30;";

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(sconn));

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new OpenApiInfo { Title = "Web API NetCore 3.1", Version = "v1" });
                c.OperationFilter<AuthorizationOperationFilter>();
                c.OperationFilter<ApiKeyHeaderOperationFilter>();
                c.OperationFilter<CancellationTokenOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                var xmlFiles = new[] { "NetCore31Api.xml", "Models.xml" };
                foreach (string xmlFile in xmlFiles)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                }
            });

            services.AddSwaggerGenNewtonsoftSupport();

            #endregion

            #region Repositories

            services.AddRepositories();

            #endregion

            #region Services

            services.AddServices();

            #endregion

            services.AddControllers();
            //ref: equals to AddMvc() at netcore 2.2 
            //services.AddControllersWithViews();
            //services.AddRazorPages();

            #region Authentication

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            #region Security Token Configuration

            var tokenConfigurations = new TokenConfigurations();

            var audience = Environment.GetEnvironmentVariable("TOKEN_AUDIENCE_CONFIG");
            var issuer = Environment.GetEnvironmentVariable("TOKEN_ISSUER_CONFIG");
            var seconds = Environment.GetEnvironmentVariable("TOKEN_SECONDS_CONFIG");

            if (!string.IsNullOrEmpty(audience))
                tokenConfigurations.Audience = audience;

            if (!string.IsNullOrEmpty(issuer))
                tokenConfigurations.Audience = issuer;

            if (!string.IsNullOrEmpty(seconds))
                tokenConfigurations.Audience = seconds;

            services.AddSingleton(tokenConfigurations);

            services.AddIdentity<User, Role>(identityConfig =>
                {
                    identityConfig.SignIn.RequireConfirmedEmail = false;
                    
                    //Password settings
                    identityConfig.Password.RequireUppercase = false;
                    identityConfig.Password.RequireNonAlphanumeric = false;
                    identityConfig.Password.RequireLowercase = false;
                    identityConfig.Password.RequiredLength = 4;
                    identityConfig.Password.RequireDigit = false;

                    //Lockout settings
                    identityConfig.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    identityConfig.Lockout.MaxFailedAccessAttempts = 5;
                    identityConfig.Lockout.AllowedForNewUsers = true;

                    //Token settings
                    identityConfig.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<User>)));
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            #endregion

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(bearerOptions =>
                {
                    var paramsValidation = bearerOptions.TokenValidationParameters;
                    paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                    paramsValidation.ValidAudience = tokenConfigurations.Audience;
                    paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;

                    paramsValidation.ValidateLifetime = true;
                    // Tempo de tolerância para a expiração de um token (utilizado
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação)
                    paramsValidation.ClockSkew = TimeSpan.Zero;
                });

            services.AddSingleton(tokenConfigurations);

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            #region Cors

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            #endregion

            #region Swagger

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/api/swagger.json", "Web Api NetCore 3.1 Doc");
                c.RoutePrefix = "api";
                c.DocumentTitle = "Api NetCore 3.1 Doc";
                c.DocExpansion(DocExpansion.List);
            });

            #endregion

            #region Middleware

            app.UseRestfulMiddleware();

            #endregion

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
