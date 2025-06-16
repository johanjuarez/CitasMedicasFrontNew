using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using CitasMedicasFront.Models;
using CitasMedicasFront.Models.Responses;
using System.Web;
using System.IO;

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

            _httpClient.BaseAddress = new Uri("https://localhost:44323/api/Auth"); 

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
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);

                // Guardamos los datos del usuario en sesión
                Session["UserId"] = userData.IdUsuario;
                Session["UserName"] = userData.Nombre;
                Session["RolId"] = userData.RolId;
                return RedirectToAction("Index", "Home");
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
