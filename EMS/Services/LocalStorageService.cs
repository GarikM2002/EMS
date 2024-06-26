using Microsoft.JSInterop;

namespace EMS.Services
{
	public class LocalStorageService(IJSRuntime jsRuntime)
	{
		private readonly IJSRuntime jsRuntime = jsRuntime;

		public async Task<string> GetAuthTokenAsync()
		{
			return await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
		}

		public async Task SetAuthTokenAsync(string token)
		{
			await jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
		}
	}
}
