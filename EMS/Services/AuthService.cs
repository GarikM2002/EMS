using Shared.DTOs;

namespace EMS.Services;

public class AuthService(EMSHttpClient httpClient, LocalStorageService localStorageService)
{
	private readonly EMSHttpClient httpClient = httpClient;
	private readonly LocalStorageService localStorageService = localStorageService;

	public async Task<HttpResponseMessage> LoginAsync(LoginViewModel loginRequest)
	{
		return await httpClient.PostAsJsonAsync("/api/Auth/login", loginRequest);
	}

	public async Task<HttpResponseMessage> RegisterAsync(RegistrationViewModel registerRequest)
	{
		return await httpClient.PostAsJsonAsync("/api/Auth/register", registerRequest);
	}

	public async Task<JWTUserData?> GetJWTUserDataAsync()
	{
		var res = await httpClient.GetFromJsonAsync<JWTUserData?>("/api/Auth/GetUserData");

		return res;
	}

	public async Task<bool> IsUserAuthenticatedAsync()
	{
		var res = await httpClient.GetAsync("/api/Auth");
		if (res.IsSuccessStatusCode)
		{
			return true;
		}
		return false;
	}

	public async Task LogoutAsync()
	{
		await localStorageService.SetAuthTokenAsync("token removed from storage.");
	}

	public async Task StoreAuthTokenAsync(string token)
	{ 
		await localStorageService.SetAuthTokenAsync(token);
	}
}
