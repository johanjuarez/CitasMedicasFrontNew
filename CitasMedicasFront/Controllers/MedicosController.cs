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

            return View(medicos);  // Devuelve la vista con los pacientes
        }

        public async Task<ActionResult> Crear()
        {
            // Definir las URLs de las APIs
            string apiConsultorios = "https://localhost:44323/api/Consultorios";
            string apiPersonal = "https://localhost:44323/api/Personal"; 

            // Realizar ambas solicitudes de forma concurrente
            var responseConsultoriosTask = _httpClient.GetStringAsync(apiConsultorios);
            var responsePersonalTask = _httpClient.GetStringAsync(apiPersonal);

            // Esperar las respuestas de ambas APIs
            await Task.WhenAll(responseConsultoriosTask, responsePersonalTask);

            // Deserializar los resultados
            var consultorios = JsonConvert.DeserializeObject<List<Consultorio>>(responseConsultoriosTask.Result);
            var personal = JsonConvert.DeserializeObject<List<Personal>>(responsePersonalTask.Result);

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

            // Obtener la lista completa de departamentos (se debería obtener solo el nombre para el id del departamento)
            var consultorioResponse = await _httpClient.GetStringAsync($"https://localhost:44323/api/Consultorios");
            var consultorios = JsonConvert.DeserializeObject<List<Consultorio>>(consultorioResponse);

            // Asegúrate de que la lista de departamentos no sea null
            ViewBag.Consultorios = consultorios ?? new List<Consultorio>();

            // Obtener el nombre del departamento a través del DepartamentoId
            var consultorio = consultorios.FirstOrDefault(d => d.ConsultorioId == medico.ConsultorioId);
            string consultorioNombre = consultorio?.Nombre ?? "Desconocido";  // Si no se encuentra, devuelve "Desconocido"
            // Pasar el nombre del departamento a la vista
            ViewBag.ConsultorioNombre = consultorioNombre;



            // Obtener la lista completa de departamentos (se debería obtener solo el nombre para el id del departamento)
            var personalResponse = await _httpClient.GetStringAsync($"https://localhost:44323/api/Personal");
            var personales = JsonConvert.DeserializeObject<List<Personal>>(personalResponse);

            // Asegúrate de que la lista de departamentos no sea null
            ViewBag.Personal = personales ?? new List<Personal>();

            // Obtener el nombre del departamento a través del DepartamentoId
            var personal = personales.FirstOrDefault(d => d.PersonalId == medico.PersonalId);
            string personalNombre = personal?.Nombre ?? "Desconocido";  // Si no se encuentra, devuelve "Desconocido"

            // Pasar el nombre del departamento a la vista
            ViewBag.PersonalNombre = personalNombre;

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

    }
}