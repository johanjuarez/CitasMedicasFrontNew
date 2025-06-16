using System.Net.Http;

namespace CitasMedicasFront.Helpers
{
    public static class HttpClientProvider
    {
        private static readonly HttpClient _httpClient;

        static HttpClientProvider()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, errors) => true
            });
        }

        public static HttpClient Client => _httpClient;
    }
}
