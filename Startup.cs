namespace API
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">CONFIG.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));

            var loggerServices = new ServiceCollection()
               .AddLogging(builder =>
               {
                   builder.AddSerilog();
                   builder.AddSeq(Configuration.GetSection("Seq"));
               });
            services.AddControllers();
            services.AddSwaggerGen(swagger =>
           {
               //This is to generate the Default UI of Swagger Documentation
               swagger.SwaggerDoc("v1", new OpenApiInfo
               {
                   Version = "v1",
                   Title = "JWT Token Authentication API",
                   Description = "API"
               });
               // To Enable authorization using Swagger (JWT)
               swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
               {
                   Name = "Authorization",
                   Type = SecuritySchemeType.ApiKey,
                   Scheme = "Bearer",
                   BearerFormat = "JWT",
                   In = ParameterLocation.Header,
                   Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
               });
               swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
               });
           });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]
                };
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("App Name", "Serilog Web App Sample")
                .CreateLogger();
            try
            {
                var startTime = DateTimeOffset.UtcNow;

                Log.Logger.Information("Started at {StartTime} and 0x{Hello:X} is hex of 42", startTime, 42);


                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseForwardedHeaders();
                }

                app.UseForwardedHeaders();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API.WebApi.xml");
                });

                app.UseRouting();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
                app.UseAuthentication();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}