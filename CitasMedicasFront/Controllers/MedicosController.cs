using CitasMedicasFront.Helpers;
using CitasMedicasFront.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CitasMedicasFront.Controllers
{
    public class MedicosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CatalogosService _catalogosService = new CatalogosService();

        public MedicosController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });
            _httpClient.BaseAddress = new Uri("https://localhost:44323/api/Medicos");
        }
        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync(""); // Obtén todos los medicos
            var medicos = JsonConvert.DeserializeObject<List<Medico>>(response);

            return View(medicos);  // Devuelve la vista con los medicos
        }

        public async Task<ActionResult> Crear()
        {
        
            // Deserializar los resultados
            var consultorios = await _catalogosService.ObtenerConsultoriosAsync();
            var personal = await _catalogosService.ObtenerPersonalAsync();

            // Pasar los datos a la vista
            ViewBag.Consultorios = consultorios;
            ViewBag.Personal = personal;

            return View(new Medico());
        }

        // POST: Personal/Crear
        [HttpPost]
        public async Task<ActionResult> Crear(Medico medico)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(medico), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");  // Redirige a la lista de personal si la creación es exitosa
                }

                return View("Error");  // Si ocurre un error, muestra una página de error
            }

            return View(medico);  // Si el modelo no es válido, permanece en la vista de Crear
        }

        public async Task<ActionResult> Guardar(Medico medico)
        {
            if (ModelState.IsValid)  // Verifica si el modelo es válido
            {

                var content = new StringContent(JsonConvert.SerializeObject(medico), Encoding.UTF8, "application/json");
                // Enviar los datos del departamento a la API para ser guardados en la base de datos
                var response = await _httpClient.PostAsync("", content);
                if (response.IsSuccessStatusCode)  // Si la creación fue exitosa
                {
                    return RedirectToAction("Index");  // Redirige a la lista de departamentos
                }
                return View("Error");  // Si ocurre un error, muestra una página de error
            }
            return View(medico);
        }

        // GET: Personal/Editar/5
        public async Task<ActionResult> Editar(int id)
        {
            // Obtener el registro de personal
            var response = await _httpClient.GetStringAsync($"?id={id}");
            var medico = JsonConvert.DeserializeObject<Medico>(response);

            if (medico == null)
            {
                return HttpNotFound();  // Si no se encuentra el registro, muestra un error 404
            }

            await AsignarConsultorioNombreAsync(medico.ConsultorioId);  // Asigna el nombre del consultorio
            await AsignarPersonalNombreAsync(medico.PersonalId);  // Asigna el nombre del personal

            return View(medico);  // Devuelve la vista con los datos del personal a editar
        }

        public async Task<ActionResult> Actualizar(Medico medico)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(medico), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"?id={medico.MedicoId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");  // Redirige a la lista de personal si la actualización es exitosa
                }

                return View("Error");  // Si ocurre un error, muestra una página de error
            }

            return View("Editar", medico);  // Cambia 'Editar' por el nombre correcto de la vista
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


        private async Task AsignarConsultorioNombreAsync(int consultorioId)
        {
            var consultorios = await _catalogosService.ObtenerConsultoriosAsync();
            ViewBag.Consultorios = consultorios;


            var consultorio = consultorios.FirstOrDefault(c => c.ConsultorioId == consultorioId);
            ViewBag.ConsultorioNombre = consultorio?.Nombre ?? "Desconocido";
        }

        private async Task AsignarPersonalNombreAsync(int personalId)
        {
            var personales = await _catalogosService.ObtenerPersonalAsync();
            ViewBag.Personal = personales;

            var personal = personales.FirstOrDefault(p => p.PersonalId == personalId);
            ViewBag.PersonalNombre = personal?.Nombre ?? "Desconocido";
        }

    }
}