using Microsoft.EntityFrameworkCore;
using TaskCollab.API.Middleware;
using TaskCollab.Application;
using TaskCollab.Infrastructure;
using TaskCollab.Infrastructure.Identity;
using TaskCollab.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services

// Couche Application
builder.Services.AddApplication();

// Couche Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Services API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuration Swagger avec support pour JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TaskCollab API",
        Version = "v1",
        Description = "API de gestion de tâches collaboratives multi-tenant"
    });
    
    // Configuration JWT pour Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header utilisant le schéma Bearer. Exemple: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Appliquer automatiquement les migrations en développement
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        
        try
        {
            var appContext = services.GetRequiredService<ApplicationDbContext>();
            appContext.Database.Migrate();
            
            var identityContext = services.GetRequiredService<IdentityDbContext>();
            identityContext.Database.Migrate();
            
            // Seeding des données initiales si nécessaire
            // await SeedData.InitializeAsync(services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Une erreur est survenue lors de la migration des bases de données.");
        }
    }
}

// Middleware d'exception
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();