using Api.Infra.CrossCutting.IoC;
using Api.Infra.Data;
using Api.Infra.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiComDDD
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.EnableEndpointRouting = false;
            });

            services.AddContextDependency();
            services.AddRepositoryDependency();

            // System.Text.Json.JsonException: A possible object cycle was detected which is not supported
            services.AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddServiceDependency();
            services.AddSwaggerDependency();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                Api.Infra.Data.Startup.Seed<ContextInMemory>(app);
            }

            /*app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });*/

            app.UseMvc();
            app.UseSwaggerDependency();
        }
    }
}
