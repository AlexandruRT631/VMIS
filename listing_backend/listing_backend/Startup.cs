using System.Text;
using listing_backend.Constants;
using listing_backend.DataAccess;
using listing_backend.Repositories;
using listing_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace listing_backend;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ListingDbContext>(options =>
            options.UseMySQL(Configuration.GetConnectionString("ListingDbConnection")!));
        
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

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<IDoorTypeService, DoorTypeService>();
        services.AddScoped<IFeatureExteriorService, FeatureExteriorService>();
        services.AddScoped<IFeatureInteriorService, FeatureInteriorService>();
        services.AddScoped<IFuelService, FuelService>();
        services.AddScoped<IMakeService, MakeService>();
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
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

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
}