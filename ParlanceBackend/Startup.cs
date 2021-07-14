using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using ParlanceBackend.Misc;
using Microsoft.AspNetCore.OData;
using ParlanceBackend.Data;
using ParlanceBackend.Services;
using System; 

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ParlanceBackend", Version = "v1" });
            });
            services.Configure<ParlanceConfiguration>(Configuration.GetSection("Parlance"));
            services.AddDbContext<ProjectContext>(options => options.UseSqlite(Utility.Parse(Configuration.GetConnectionString("ProjectContext"))));
            services.AddSingleton<GitService>();
            services.AddSingleton<TranslationFileService>();
            services.AddHostedService<GitPushService>();

            // services.AddDbContext<ProjectContext>(options => options.UseSqlite("Data Source=database.db;"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParlanceBackend v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
