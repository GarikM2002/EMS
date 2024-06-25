using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.DTOs;

namespace EMS.Services;

public class AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
{
	private readonly HttpClient httpClient = httpClient;
	private readonly IJSRuntime jsRuntime = jsRuntime;

	public async Task<HttpResponseMessage> LoginAsync(LoginViewModel loginRequest)
	{
		return await httpClient.PostAsJsonAsync("/api/Auth/login", loginRequest);
	}

	public async Task<HttpResponseMessage> RegisterAsync(RegistrationViewModel registerRequest)
	{
		return await httpClient.PostAsJsonAsync("/api/Auth/register", registerRequest);
	}

	public async Task<bool> IsUserAuthenticated()
	{
		var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
		if (string.IsNullOrEmpty(token))
		{
			return false;
		}
		return false;
	}

	public async Task LogoutAsync()
	{
		await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
	}
}
