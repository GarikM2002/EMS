namespace EMS.Services;

public class EMSHttpClient
{
	const string EMSApi = "https://localhost:7216";
	private readonly LocalStorageService localStorageService;
	private readonly HttpClient httpClient;

	public EMSHttpClient(LocalStorageService localStorageService,
		HttpClient httpClient)
	{
		this.localStorageService = localStorageService;
		this.httpClient = httpClient;
		this.httpClient.BaseAddress = new(EMSApi);
	}

	public async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string? requestUri, TValue value)
	{
		await AddHeadersAsync();

		return await httpClient.PostAsJsonAsync(requestUri, value);
	}

	public async Task<HttpResponseMessage> PutAsJsonAsync<TValue>(string? requestUri, TValue value)
	{
		await AddHeadersAsync();

		return await httpClient.PutAsJsonAsync(requestUri, value);
	}

	public async Task<HttpResponseMessage> DeleteAsync(string? requestUri)
	{
		await AddHeadersAsync();

		return await httpClient.DeleteAsync(requestUri);
	}

	public async Task<TValue?> GetFromJsonAsync<TValue>(string? requestUri)
	{
		await AddHeadersAsync();

		return await httpClient.GetFromJsonAsync<TValue?>(requestUri);
	}

	public async Task<HttpResponseMessage> GetAsync(string? requestUri)
	{
		await AddHeadersAsync();

		return await httpClient.GetAsync(requestUri);
	}

	private async Task AddHeadersAsync()
	{
		var token = await localStorageService.GetAuthTokenAsync();
		if (!string.IsNullOrEmpty(token))
		{
			//httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
			httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
		}
	}
}
