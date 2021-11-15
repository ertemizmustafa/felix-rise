using Felix.Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Configuration
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddCommonSetup(this IServiceCollection services, IConfiguration configuration, bool useJwt = true, string swaggerTitle = "", bool isGateway = false)
        {
            var identitySettings = configuration.GetSection("IdentitySettings").Get<IdentitySettings>();

            if (identitySettings.Cors != null)
            {
                services.AddCors();
            }

            if (useJwt)
            {
                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(identitySettings.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                    };
                });
            }

            services.Configure<IdentitySettings>(options => configuration.GetSection("IdentitySettings").Bind(options));
            services.Configure<AppSettings>(options => configuration.GetSection("AppSettings").Bind(options));

            if (!isGateway)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerTitle, Version = "v1" });
                    if (useJwt)
                    {
                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = "Example: 'Bearer 123123123'",
                            Name= "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer"

                        });
                        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,
                                },new List<string>() }
                        });
                    }
                });
            }

            return services;
        }

        public static IApplicationBuilder ApplyCommonSetup(this IApplicationBuilder app, IConfiguration configuration, string swaggerTitle = "", bool isGateway = false)
        {
            var identitySettings = configuration.GetSection("IdentitySettings").Get<IdentitySettings>();

            if (identitySettings.Cors != null)
            {
                app.UseCors(options => options.WithOrigins(identitySettings.Cors).AllowCredentials().AllowAnyHeader().AllowAnyMethod());
            }

            if(!isGateway)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocExpansion(DocExpansion.None);
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", swaggerTitle);
                });
            }

            return app;
        }
    }
}
