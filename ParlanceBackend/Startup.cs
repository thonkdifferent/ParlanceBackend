using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using ParlanceBackend.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Misc;
using ParlanceBackend.Services;

namespace ParlanceBackend
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
            //services.AddDbContext<ProjectContext>(opt => opt.UseInMemoryDatabase("VictorsProjectCollection"));
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest).AddOData();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ParlanceBackend", Version = "v1" });
            });
            services.Configure<ParlanceConfiguration>(Configuration.GetSection("Parlance"));
            services.AddDbContext<ProjectContext>(options => options.UseSqlite(Configuration.GetConnectionString("ProjectContext")));

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
