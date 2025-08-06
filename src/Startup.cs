using ApiCredit.Services.Interfaces;
using ApiCredit.Services.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Prometheus;
using Prometheus.SystemMetrics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ApiCredit
{
    public class Startup
    {
        public IConfiguration _configuration { get; private set; }
        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add health checks
            services.AddControllers(options =>
            {
            }).AddNewtonsoftJson(options =>
            {
                options.UseCamelCasing(true);
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.None;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Security:issuer"],
                        ValidAudience = _configuration["Security:audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:key"]))
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Menu",
                    Version = _configuration["Program:AppVersion"],
                    Description = $"api {_configuration["Program:AppVersion"]}",
                    TermsOfService = new Uri("https://gov.pt/")
                });
                c.OrderActionsBy((apiDesc) =>
                {
                    return $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.ActionDescriptor.RouteValues["action"]}_{apiDesc.HttpMethod}";
                });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(_env.ContentRootPath, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddHttpClient();
            services.Configure<Config>(_configuration.GetSection("Program"));

            services.AddScoped<IDigitalKeyService, DigitalKeyServices>();
            services.AddScoped<IRiskService, RiskService>();
            services.AddScoped<ICreditService, CreditService>();

            services.AddSystemMetrics();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMetricServer();
            app.UseRouting();
            app.UseHttpMetrics(options =>
            {
                options.AddCustomLabel("host", context => context.Request.Host.Host);
                options.AddCustomLabel("appVersion", context =>
                {
                    var config = context.RequestServices.GetService<IConfiguration>();
                    return config["Program:appVersion"];
                });
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapControllers();
               
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Credit");
            });
        }
    }
}
