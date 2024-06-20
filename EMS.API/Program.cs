using DataAccess;
using DataAccess.Enities;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using EMS.API.Helpers;
using Services.Auth;
using Services.Configurations;
using Services.Contracts;
using Services.Employees;
using Services.Employers;
using Shared.DTOs;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? throw new Exception("JwtSettings wasn't found");
builder.Services.AddJwtAuthentication(jwtSettings);
builder.Services.AddAuthorization();

builder.Services.AddScoped(provider =>
    new DataContext(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection wasn't found.")));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddSingleton(new JwtTokenService(jwtSettings));
builder.Services.AddAutoMapper([typeof(LoginViewModel).Assembly, typeof(Employer).Assembly]);
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployerService, EmployerService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
