using EMS.Components;
using EMS.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MudBlazor.Services;
using Shared.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddHttpClient<EMSHttpClient>();
builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddValidatorsFromAssemblyContaining<LoginViewModelValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
