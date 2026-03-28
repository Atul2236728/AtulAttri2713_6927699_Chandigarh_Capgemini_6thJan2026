using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ProductClient.Pages
{
    public class ApiHealthModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public bool IsHealthy { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public List<string> Endpoints { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;

        public ApiHealthModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task OnGetAsync()
        {
            try
            {
                var client = _clientFactory.CreateClient("ProductApi");
                var response = await client.GetAsync("api/apihealth");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json).RootElement;

                    IsHealthy = true;
                    Status = doc.GetProperty("status").GetString() ?? "";
                    Timestamp = doc.GetProperty("timestamp").GetString() ?? "";
                    Version = doc.GetProperty("version").GetString() ?? "";
                    Endpoints = doc.GetProperty("endpoints")
                                   .EnumerateArray()
                                   .Select(e => e.GetString() ?? "")
                                   .ToList();
                }
                else
                {
                    IsHealthy = false;
                    ErrorMessage = $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                IsHealthy = false;
                ErrorMessage = ex.Message;
            }
        }
    }
}
