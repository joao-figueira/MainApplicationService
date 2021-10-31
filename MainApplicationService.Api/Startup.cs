using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using MainApplicationService.Api.Configurations;
using MainApplicationService.Api.Mappers;
using MainApplicationService.Api.Middlewares;
using MainApplicationService.Helpers;
using MainApplicationService.Interfaces;
using MainApplicationService.Providers;
using MainApplicationService.Repositories;
using MainApplicationService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Session;

namespace MainApplicationService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var settings = Configuration.GetSection("RavenSettings").Get<RavenSettings>();
            if (settings == null || string.IsNullOrEmpty(settings.CertificatePath))
            {
                throw new InvalidOperationException("Invalid RavenDB Settings");
            }

            var offensiveExpressionsAsString = File.ReadAllText("OffensiveExpressionsList.txt");
            OffensiveContentHelper.SetOffensiveExpressions(offensiveExpressionsAsString);

            var store = new DocumentStore
            {
                Urls = settings.Urls,
                Database = settings.DatabaseName,
                Certificate = new X509Certificate2(settings.CertificatePath, settings.CertificatePassword),
                Conventions = new DocumentConventions()
                {
                    UseOptimisticConcurrency = true
                }
            };

            store.Initialize();

            services.AddSwaggerDocument(c =>
            {
                c.Title = "Service API";
                c.Version = "1.0";
                c.OperationProcessors.Add(new AddRequiredAccessKeyHeaderParameterProcess());
            });

            services.AddScoped<IAsyncDocumentSession>(serviceProvider => serviceProvider
                .GetRequiredService<IDocumentStore>()
                .OpenAsyncSession());

            services.AddSingleton<IDocumentStore>(store);
            services.AddScoped<ICommentsMapper, CommentsMapper>();
            services.AddScoped<ICommentsValidationService, CommentsValidationService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<ICommentsRepository, CommentsRavenDbRepository>();
            services.AddScoped<IEntityBaseRepository, EntityBaseRavenDbRepository>();
            services.AddScoped<IArticlesRepository, ArticlesRavenDbRepository>();
            services.AddScoped<CurrentUserProvider>(); //supposing that the app supports authentication and sets the current user 
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = ApiVersion.Default;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCurrentUserMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

        }

        /*
         * In a real case this would be an authentication token instead of a userId
         */
        public class AddRequiredAccessKeyHeaderParameterProcess : IOperationProcessor
        {
            public bool Process(OperationProcessorContext context)
            {
                context.OperationDescription.Operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "UserId",
                    Kind = OpenApiParameterKind.Header,
                    Schema = new JsonSchema { Type = JsonObjectType.String },
                    IsRequired = true
                });
                return true;
            }
        }
    }
}
