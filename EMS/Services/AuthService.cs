using Microsoft.AspNetCore.Identity.Data;
using Shared.DTOs;

namespace EMS.Services;

public class AuthService
{
	private readonly HttpClient httpClient;

	public AuthService(HttpClient httpClient)
	{
		this.httpClient = httpClient;
	}

	public async Task<HttpResponseMessage> LoginAsync(LoginViewModel loginRequest)
	{
		return await httpClient.PostAsJsonAsync("/api/Auth/login", loginRequest);
	}

	public async Task<HttpResponseMessage> RegisterAsync(RegistrationViewModel registerRequest)
	{
		return await httpClient.PostAsJsonAsync("/api/Auth/register", registerRequest);
	}
}
