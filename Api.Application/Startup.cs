using Api.CrossCutting.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Api.Application
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

            ConfigureService.ConfigureDependenciesService(services);
            ConfigureRepository.ConfigureDependenciesRepository(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Api Restaurant App",
                    Description = "Api of Restaurant Application",
                    Contact = new OpenApiContact
                    {
                        Name = "Matheus de Almeida Oliveira",
                        Email = "9852matheus@gmail.com"
                    }
                });
            });  

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                });
            });  
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
                app.UseHsts();
            }

            app.UseCors("EnableCORS");

            app.UseHttpsRedirection();
            //app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant Api");
                c.RoutePrefix = string.Empty;
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
