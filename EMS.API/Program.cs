using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using EMS.API.Helpers;
using EMS.API.Models;
using EMS.API.Services;

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
