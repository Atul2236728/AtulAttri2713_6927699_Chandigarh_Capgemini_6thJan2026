using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace HospitalWeb.Services;

public class ApiService
{
    private readonly IHttpClientFactory _factory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
    {
        _factory = factory;
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpClient CreateClient()
    {
        var client = _factory.CreateClient("HospitalAPI");
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var client = CreateClient();
        var response = await client.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode) return default;
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task<(bool Success, string? Json, string? Error)> PostAsync(string endpoint, object data)
    {
        var client = CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(endpoint, content);
        var json = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode
            ? (true, json, null)
            : (false, null, ExtractError(json));
    }

    public async Task<(bool Success, string? Error)> PutAsync(string endpoint, object data)
    {
        var client = CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await client.PutAsync(endpoint, content);
        var json = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode ? (true, null) : (false, ExtractError(json));
    }

    public async Task<(bool Success, string? Error)> PatchAsync(string endpoint, object data)
    {
        var client = CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Patch, endpoint) { Content = content };
        var response = await client.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode ? (true, null) : (false, ExtractError(json));
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(string endpoint)
    {
        var client = CreateClient();
        var response = await client.DeleteAsync(endpoint);
        var json = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode ? (true, null) : (false, ExtractError(json));
    }

    private static string ExtractError(string json)
    {
        try
        {
            dynamic? obj = JsonConvert.DeserializeObject(json);
            return obj?.message ?? obj?.details ?? "An error occurred.";
        }
        catch { return json; }
    }
}
