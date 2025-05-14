using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CitasMedicasFront.Models;
using System.Text;

namespace CitasMedicasFront.Controllers
{
    public class RolesController : Controller
    {
        private readonly HttpClient _httpClient;

        public RolesController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });
            _httpClient.BaseAddress = new Uri("https://localhost:44323/api/Roles");
        }

        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("");
            var roles = JsonConvert.DeserializeObject<List<Rol>>(response);
            return View(roles);
        }

        public ActionResult Crear()
        {
            return View(new Rol());
        }
        public async Task<ActionResult> Guardar(Rol rol)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(rol), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }
            return View(rol);
        }
        public async Task<ActionResult> Editar(int id)
        {
            var response = await _httpClient.GetStringAsync($"?id={id}");
            var rol = JsonConvert.DeserializeObject<Rol>(response);
            if (rol == null)
            {
                return HttpNotFound();  // Si no se encuentra el Rol, se devuelve un error 404
            }
            return View(rol);
        }

        public async Task<ActionResult> Actualizar(Rol rol)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(rol), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"?id={rol.RolId}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }
            return View(rol);
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var response = await _httpClient.DeleteAsync($"?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Error");
        }
    }

}