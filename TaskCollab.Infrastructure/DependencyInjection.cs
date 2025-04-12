using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TaskCollab.Application.Interfaces;
using TaskCollab.Domain.Interfaces;
using TaskCollab.Infrastructure.Identity;
using TaskCollab.Infrastructure.Persistence;
using TaskCollab.Infrastructure.Persistence.Interceptors;
using TaskCollab.Infrastructure.Repositories;
using TaskCollab.Infrastructure.Services;

namespace TaskCollab.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration des interceptors
        services.AddScoped<TenantInterceptor>();
        services.AddScoped<AuditableEntityInterceptor>();
        
        // Configuration de la base de données principale
        services.AddDbContext<ApplicationDbContext>((sp, options) => {
            var tenantInterceptor = sp.GetRequiredService<TenantInterceptor>();
            var auditableEntityInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            );
        });
        
        // Configuration de la base de données d'identité
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("IdentityConnection"),
                b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)
            )
        );
        
        // Configuration d'Identity
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();
            
        // Configuration JWT
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        
        services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            
        // Services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IIdentityService, IdentityService>();
        
        // Repositories
        // Repositories
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantUserRepository, TenantUserRepository>();
        
        // Autres configurations selon les besoins
        services.AddHttpContextAccessor();
        
        return services;
    }
}