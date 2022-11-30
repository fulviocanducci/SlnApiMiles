using API.DataAccess;
using API.Repositories;
using API.Services.Token;
using API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using Canducci.GeneratePassword.Extensions.DependencyInjection;
namespace API
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
            services.AddDbContext<DatabaseAccess>(x => x.UseMySQL(Configuration.GetConnectionString("Database")));
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddGeneratePassword();
            services.AddCors();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Secret.Bytes),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Backup Miles",
                    Version = "v1",
                    Description = "API Backup Miles",
                    Contact = new OpenApiContact
                    {
                        Name = "Fúlvio Cezar Canducci Dias",
                        Email = string.Empty,
                        Url = new Uri("https://api.s2vip.com.br/"),
                    },
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Description = "Authorization Miles API",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            services.Configure<RouteOptions>(configuration =>
            {
                configuration.LowercaseQueryStrings = true;
                configuration.LowercaseUrls = true;
            });
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Miles");
                c.RoutePrefix = string.Empty;
            });
            app.UseReDoc(c =>
            {
                c.DocumentTitle = "REDOC API Documentation";
                c.SpecUrl = "/swagger/v1/swagger.json";
                c.EnableUntrustedSpec();                
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
