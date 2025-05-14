using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using CitasMedicasFront.Models;

namespace CitasMedicasFront.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                // Ignora la validación del certificado SSL
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            _httpClient.BaseAddress = new Uri("https://localhost:44323/api/Auth");  // URL de tu API
        }

        // GET: Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        public async Task<ActionResult> Login(UsuarioLogin loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Auth/Login", content);  // Llamada a la API

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                Session["UserToken"] = token;  // Guarda el token en sesión
                return RedirectToAction("Index", "Home");  // Redirige al inicio
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";  // Muestra error
            return View();
        }

        // GET: Auth/Logout
        public ActionResult Logout()
        {
            Session["UserToken"] = null;  // Elimina sesión
            return RedirectToAction("Login");
        }
    }
}
