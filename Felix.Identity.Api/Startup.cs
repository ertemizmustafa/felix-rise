using Felix.Common.Configuration;
using Felix.Common.Helpers;
using Felix.Data.Core.Configuration;
using Felix.Identity.Api.Services;
using Felix.Identity.Api.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Felix.Identity.Api
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
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = c =>
                    {
                        var errors = string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                            .SelectMany(v => v.Errors)
                            .Select(v => v.ErrorMessage));
                        return new OkObjectResult(EnvelopeHelper.ToEnvelope("", Common.Enums.ResponseEnum.BadRequest, false, errors));
                    };
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UserInfoModelValidation>())
                .Services.AddTransient<IValidatorInterceptor, ValidationHelper>()
                .AddHttpContextAccessor();
            services.Configure<RequestLocalizationOptions>(opt => opt.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr-TR"));

            services.AddCommonSetup(Configuration, useJwt: false, swaggerTitle: "Identity API");
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddDatabaseConnection("connectionsring");
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
           
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplyCommonSetup(Configuration, swaggerTitle: "Identity API");
            app.UseHttpsRedirection();
            app.UseRouting();
            ApplicationContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            app.UseAuthorization();
            app.UseMiddleware<ExceptionHelper>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
