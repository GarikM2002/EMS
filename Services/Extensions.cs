using System.Text;
using DataAccess;
using DataAccess.Enities;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Services.Auth;
using Services.Contracts;
using Services.Employees;
using Services.Employers;
using Services.Configurations;
using Shared.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Services;

public static class Extensions
{
    public static void AddEMSServices(this IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = config.GetSection("Jwt").Get<JwtSettings>() ?? throw new Exception("JwtSettings wasn't found");
        services.AddJwtAuthentication(jwtSettings);
        services.AddAuthorization();

        services.AddScoped(provider =>
            new DataContext(config.GetConnectionString("DefaultConnection")
            ?? throw new Exception("Connection wasn't found.")));
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployerRepository, EmployerRepository>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddSingleton(new JwtTokenService(jwtSettings));
        services.AddAutoMapper([typeof(LoginViewModel).Assembly, typeof(Employer).Assembly]);
        services.AddScoped<AuthenticationService>();
        services.AddScoped<IContractService, ContractService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployerService, EmployerService>();
    }

    public static void AddJwtAuthentication(this IServiceCollection services,
            JwtSettings jwtSettings)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret))
            };
        });
    }
}
