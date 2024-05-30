using System.Text;
using listing_backend.Constants;
using listing_backend.DataAccess;
using listing_backend.Repositories;
using listing_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace listing_backend;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ListingDbContext>(options =>
            options.UseLazyLoadingProxies()
                .UseMySQL(Configuration.GetConnectionString("ListingDbConnection")!));
        
        services.AddScoped<DbContext>(provider => provider.GetService<ListingDbContext>()!);

        services.AddAutoMapper(typeof(Startup));

        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IDoorTypeRepository, DoorTypeRepository>();
        services.AddScoped<IEngineRepository, EngineRepository>();
        services.AddScoped<IFeatureExteriorRepository, FeatureExteriorRepository>();
        services.AddScoped<IFeatureInteriorRepository, FeatureInteriorRepository>();
        services.AddScoped<IFuelRepository, FuelRepository>();
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IMakeRepository, MakeRepository>();
        services.AddScoped<IModelRepository, ModelRepository>();
        services.AddScoped<ITractionRepository, TractionRepository>();
        services.AddScoped<ITransmissionRepository, TransmissionRepository>();

        services.AddScoped<ICarService, CarService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<IDoorTypeService, DoorTypeService>();
        services.AddScoped<IEngineService, EngineService>();
        services.AddScoped<IFeatureExteriorService, FeatureExteriorService>();
        services.AddScoped<IFeatureInteriorService, FeatureInteriorService>();
        services.AddScoped<IFuelService, FuelService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IListingService, ListingService>();
        services.AddScoped<IMakeService, MakeService>();
        services.AddScoped<IModelService, ModelService>();
        services.AddScoped<ITractionService, TractionService>();
        services.AddScoped<ITransmissionService, TransmissionService>();
        
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

        services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 300 * 1000 * 1000; // 300 MB
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
        else
        {
            app.UseHsts();
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
                                { "listingDto", new OpenApiSchema { Type = "string", Format = "json" } },
                                { "images", new OpenApiSchema { Type = "array", Items = new OpenApiSchema { Type = "string", Format = "binary" } } }
                            },
                            Required = new HashSet<string> { "listingDto", "images" }
                        }
                    }
                }
            };
        }
    }
}