using Data.Database;
using Logic.Administration.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Administartion;
using System.Text;
using Microsoft.OpenApi;
using Shared.Models.User;
using Logic.Shared.DI;
using Shared.Models.Email;
using Logic.Tasks.DI;

namespace Core.Api.Bundels
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration, string corsPolicy)
        {
            services.Configure<JwtTokenModel>(configuration.GetSection("Jwt"));
            services.Configure<UserModel>(configuration.GetSection("DefaultAdminUser"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<ApiOptions>(configuration.GetSection("ApiOptions"));

            var connectionString = configuration.GetConnectionString("TaskPlannerDb") ?? null;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            } 

            services.AddDbContext<DatabaseContext>(options => options.UseMySQL(connectionString));

            services.AddSharedServices();
            services.AddAdministrationServices();
            services.AddTaskServices();

            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var jwtConfig = configuration.GetSection("Jwt").Get<JwtTokenModel>();

            if (jwtConfig == null)
            {
                throw new InvalidOperationException("JWT configuration section is missing or invalid.");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var key = jwtConfig?.SecurityKey ?? string.Empty;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig?.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            RegisterSwagger(services);
        }

        private static void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TaskPlanner API",
                    Version = "v1",
                    Description = "API for TaskPlanner-Anwendung",
                    Contact = new OpenApiContact
                    {
                        Name = "Manuel Peise",
                        Email = "manuel.p80@gmx.de"
                    }
                });

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = new List<string>()
                });
            });
        }
    }
}
