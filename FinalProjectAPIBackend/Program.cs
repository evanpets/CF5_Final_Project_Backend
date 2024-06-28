
using AutoMapper;
using DotNetEnv;
using FinalProjectAPIBackend.Configuration;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.Helpers;
using FinalProjectAPIBackend.Repositories;
using FinalProjectAPIBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Security.Claims;
using System.Text;

namespace FinalProjectAPIBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");


            builder.Services.AddDbContext<FinalProjectAPIBackendDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IApplicationService, ApplicationService>();

            builder.Services.AddRepositories();
            builder.Services.AddScoped(provider =>
            new MapperConfiguration(config =>
            {
                config.AddProfile(new MapperConfig());
            })
            .CreateMapper());

            var key = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidIssuer = "https://localhost:5001",

                    ValidateAudience = true,
                    ValidAudience = "https://localhost:4200",

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    // return new JsonWebToken(token); in .NET 8
                    // Override the default token signature validation an do NOT validate the signature
                    // Just return the token
                    //SignatureValidator = (token, validator) => { return new JsonWebToken(token); }
                };
            });

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Management API", Version = "v1" });
                // Non-nullable reference are properly documented
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "FinalProjectAPIBackend.xml"));
                options.SupportNonNullableReferenceTypes();
                options.OperationFilter<AuthorizeOperationFilter>();
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT"
                    });
            });

            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll",
                    b => b.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin());
            });

            builder.Services.AddCors(options => {
                options.AddPolicy("AngularClient",
                     b => b.WithOrigins("https://localhost:4200") // Assuming Angular runs on localhost:4200
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
                RequestPath = "/Uploads"
            });
            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
