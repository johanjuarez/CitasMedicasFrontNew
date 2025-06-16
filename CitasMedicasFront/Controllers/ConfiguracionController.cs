using CitasMedicasFront.Helpers;
using CitasMedicasFront.Models;
using CitasMedicasFront.Models.DTOS;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CitasMedicasFront.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly HttpClient _httpClient;


        public ConfiguracionController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

        }
        public async Task<ActionResult> Perfil()
        {
            int usuarioId = Convert.ToInt32(Session["UserId"]);
            string url = $"{ApiUrls.Usuarios}/GetUsuarioConPersonal/{usuarioId}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var contenido = await response.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<UsuarioPersonalDTO>(contenido);

            return View(usuario);
        }


    }
}