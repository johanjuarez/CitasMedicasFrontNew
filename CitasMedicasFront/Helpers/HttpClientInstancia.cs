using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace CitasMedicasFront.Helpers
{
    public static class HttpClientInstancia
    {
        private static readonly HttpClient _cliente;

        static HttpClientInstancia()
        {
            _cliente = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            _cliente.BaseAddress = new Uri(ApiUrls.Base);
        }

        public static HttpClient Instancia => _cliente;
    }
}