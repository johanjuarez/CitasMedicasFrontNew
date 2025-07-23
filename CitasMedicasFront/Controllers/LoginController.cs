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

            _httpClient.BaseAddress = new Uri("https://localhost:44323/api/Auth/");

        }

        // GET: Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult VerificarCodigo()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UsuarioLogin loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Login", content);  // Llamada a la API

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);

                // Guardamos los datos del usuario en sesión
                Session["UserId"] = userData.IdUsuario;
                Session["UserName"] = userData.Nombre;
                Session["RolId"] = userData.RolId;
                Session["Usuario"] = userData.Usuario;

                // Guardar el usuario temporalmente para usarlo en la vista VerificarCodigo
                TempData["UserName"] = userData.Usuario;

                // Redirigir a la pantalla de validación de código
                return RedirectToAction("VerificarCodigo", "Login");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }


        // POST: Verificar Código
        [HttpPost]
        public async Task<ActionResult> VerificarCodigo(string codigo)
        {
            var usuario = Session["UserName"]?.ToString();

            if (string.IsNullOrEmpty(usuario))
            {
                ViewBag.Error = "Sesión expirada. Inicia sesión de nuevo.";
                return RedirectToAction("Login");
            }

            var datos = new
            {
                Usuario = usuario,
                Codigo = codigo
            };

            var contenido = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "application/json");
            var respuesta = await _httpClient.PostAsync("VerificacionCodigo", contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Código incorrecto.";
            TempData.Keep("usuario");
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
