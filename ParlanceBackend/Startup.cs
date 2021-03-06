using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using ParlanceBackend.Misc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Misc;
using ParlanceBackend.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ParlanceBackend.Authentication;

namespace ParlanceBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Console.WriteLine("Parlance version GIT-MASTER branch XDocument");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ProjectContext>(opt => opt.UseInMemoryDatabase("VictorsProjectCollection"));
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest).AddOData();
            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = DBusAuthenticationHandler.SchemeName;
            }).AddScheme<AuthenticationSchemeOptions, DBusAuthenticationHandler>(DBusAuthenticationHandler.SchemeName,
                options => { });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(ProjectsAuthorizationHandler.UpdateTranslationFilePermission,
                    policy => policy.Requirements.Add(ProjectsAuthorizationHandler.UpdateTranslationFile));
            });
            services.AddScoped<IAuthorizationHandler, ProjectsAuthorizationHandler>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ParlanceBackend", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
            services.Configure<ParlanceConfiguration>(Configuration.GetSection("Parlance"));
            services.AddDbContext<ProjectContext>(options => options.UseSqlite(Utility.Parse(Configuration.GetConnectionString("ProjectContext"))));
            services.AddSingleton<GitService>();
            services.AddSingleton<TranslationFileService>();
            services.AddHostedService<GitPushService>();

            // services.AddDbContext<ProjectContext>(options => options.UseSqlite("Data Source=database.db;"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ParlanceConfiguration> parlanceConfiguration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParlanceBackend v1"));
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Utility.Parse(parlanceConfiguration.Value.StaticFilesPath))
                {
                    
                },
                RequestPath = ""
            });
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
