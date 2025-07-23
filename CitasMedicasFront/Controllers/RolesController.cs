using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CitasMedicasFront.Helpers;
using CitasMedicasFront.Models;

namespace CitasMedicasFront.Controllers
{
    public class RolesController : Controller
    {
        private readonly HttpClient _httpClient;

        public RolesController()
        {
            // Usamos la instanica 
            _httpClient = HttpClientInstancia.Instancia;
        }

        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync(ApiUrls.Roles);
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
                var response = await _httpClient.PostAsync(ApiUrls.Roles, content);
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
            var response = await _httpClient.GetStringAsync($"{ApiUrls.Roles}?id={id}");
            var rol = JsonConvert.DeserializeObject<Rol>(response);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        public async Task<ActionResult> Actualizar(Rol rol)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(rol), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{ApiUrls.Roles}?id={rol.RolId}", content);
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
            var response = await _httpClient.DeleteAsync($"{ApiUrls.Roles}?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Error");
        }
    }
}
