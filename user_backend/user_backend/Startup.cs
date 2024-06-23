using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using user_backend.Constants;
using user_backend.DataAccess;
using user_backend.Repositories;
using user_backend.Services;

namespace user_backend;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseMySQL(Configuration.GetConnectionString("UserDbConnection")!));
        
        services.AddScoped<DbContext>(provider => provider.GetService<UserDbContext>()!);
        
        services.AddAutoMapper(typeof(Startup));

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IImageService, ImageService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context => 
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "text/plain";
                        return context.Response.WriteAsync(ExceptionMessages.InvalidToken);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "text/plain";
                        return context.Response.WriteAsync(ExceptionMessages.ForbiddenUser);
                    }
                };
            });
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
        
        services.AddControllers();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
            c.OperationFilter<AddFileParamTypes>();
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseCors("AllowAllOrigins");
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = "api/swagger";
        });
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
    
    private class AddFileParamTypes : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(List<IFormFile>))
                .ToList();

            if (!fileParams.Any()) return;

            operation.Parameters.Clear();

            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                { "user", new OpenApiSchema { Type = "string", Format = "json" } },
                                { "profileImage", new OpenApiSchema { Type = "array", Items = new OpenApiSchema { Type = "string", Format = "binary" } } }
                            },
                            Required = new HashSet<string> { "user", "profileImage" }
                        }
                    }
                }
            };
        }
    }
}