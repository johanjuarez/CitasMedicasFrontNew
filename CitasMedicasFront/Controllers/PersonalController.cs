using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using CitasMedicasFront.Models;
using System;
using CitasMedicasFront.Helpers;
using System.Linq;

namespace CitasMedicasFront.Controllers
{
    public class PersonalController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CatalogosService _catalogosService = new CatalogosService();

        public PersonalController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            _httpClient.BaseAddress = new Uri(ApiUrls.Personal);
        }

        // GET: Personal
        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync(""); // Obtén todos los registros de personal
            var personalList = JsonConvert.DeserializeObject<List<Personal>>(response);

            return View(personalList);  // Devuelve la vista con los registros de personal
        }

        // GET: Personal/Crear
        public async Task<ActionResult> Crear()
        {

            var departamentos = await _catalogosService.ObtenerDepartamentosAsync();

            ViewBag.Departamentos = departamentos;

            return View(new Personal());
        }


        // POST: Personal/Crear
        [HttpPost]
        public async Task<ActionResult> Crear(Personal personal)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(personal), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");  // Redirige a la lista de personal si la creación es exitosa
                }

                return View("Error");  // Si ocurre un error, muestra una página de error
            }

            return View(personal);  // Si el modelo no es válido, permanece en la vista de Crear
        }

        public async Task<ActionResult> Guardar(Personal personal)
        {
            if (ModelState.IsValid)  // Verifica si el modelo es válido
            {

                var content = new StringContent(JsonConvert.SerializeObject(personal), Encoding.UTF8, "application/json");
                // Enviar los datos del departamento a la API para ser guardados en la base de datos
                var response = await _httpClient.PostAsync("", content);
                if (response.IsSuccessStatusCode)  // Si la creación fue exitosa
                {
                    return RedirectToAction("Index");  // Redirige a la lista de departamentos
                }
                return View("Error");  // Si ocurre un error, muestra una página de error
            }
            return View(personal);
        }
     

        // GET: Personal/Editar/5
        public async Task<ActionResult> Editar(int id)
        {
            // Obtener el registro de personal
            var response = await _httpClient.GetStringAsync($"?id={id}");
            var personal = JsonConvert.DeserializeObject<Personal>(response);

            if (personal == null)
            {
                return HttpNotFound();  
            }

            await AsignarNombreDepartamentoAsync(personal.DepartamentoId);


            return View(personal);  
        }

        public async Task<ActionResult> Actualizar(Personal personal)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(personal), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"?id={personal.PersonalId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");  // Redirige a la lista de personal si la actualización es exitosa
                }

                return View("Error");  // Si ocurre un error, muestra una página de error
            }

            return View("Editar", personal);  // Cambia 'Editar' por el nombre correcto de la vista
        }


        // GET: Personal/Eliminar/5
        public async Task<ActionResult> Eliminar(int id)
        {
            var response = await _httpClient.DeleteAsync($"?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Error");
        }

        private async Task AsignarNombreDepartamentoAsync(int departamentoId)
        {
            var departamentos = await _catalogosService.ObtenerDepartamentosAsync();


            var departamento = departamentos.FirstOrDefault(d => d.DepartamentoId == departamentoId);
            string departamentoNombre = departamento?.Nombre ?? "Desconocido";
            ViewBag.DepartamentoNombre = departamentoNombre;
        }


    }
}
