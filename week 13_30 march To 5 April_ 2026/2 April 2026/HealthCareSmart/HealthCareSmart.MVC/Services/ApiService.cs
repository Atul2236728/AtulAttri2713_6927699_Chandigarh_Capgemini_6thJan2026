namespace HealthCareSmart.MVC.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _accessor;

        public ApiService(IHttpClientFactory factory, IHttpContextAccessor accessor)
        {
            _factory = factory;
            _accessor = accessor;
        }

        public HttpClient GetAuthenticatedClient()
        {
            var client = _factory.CreateClient("API");
            var token = _accessor.HttpContext?.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}