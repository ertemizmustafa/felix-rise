using Felix.Common.Configuration;
using Felix.Common.Helpers;
using Felix.Gateway.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt => opt.EnableEndpointRouting = false)
                .AddNewtonsoftJson(opt =>
                {
                    opt.UseMemberCasing();
                })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            services.Configure<RequestLocalizationOptions>(opt => opt.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr-TR"))
                .AddHttpContextAccessor();

            services.AddCommonSetup(Configuration, isGateway: true);
            services.AddOcelot(Configuration);
            //.AddCacheManager(x =>
            //{
            //    x.WithDictionaryHandle();
            //});

            services.AddSwaggerForOcelot(Configuration);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FELIX Gateway", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplyCommonSetup(Configuration, isGateway: true);
            app.UseMiddleware<GatewayRequestHandler>();
            app.UsePathBase("/gateway");
            app.UseHttpsRedirection();
            app.UseRouting();
            ApplicationContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("FELIX Gateway!");
                });
            });

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                opt.DownstreamSwaggerEndPointBasePath = "/gateway/swagger/docs";
                opt.PathToSwaggerGenerator = "/swagger/docs";
                opt.ServerOcelot = "/gateway";
            }).UseOcelot().Wait();
        }
    }
}
