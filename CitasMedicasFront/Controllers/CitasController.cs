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
    public class CitasController : Controller
    {
        private readonly HttpClient _httpClient;
        public CitasController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            _httpClient.BaseAddress = new Uri("https://localhost:44323/api/Citas");
        }
        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync(""); // Obtén todas las citas
            var citas = JsonConvert.DeserializeObject<List<Cita>>(response);

            return View(citas);  // Devuelve la vista con los pacientes
        }
        public async Task<ActionResult> Crear()
        {
            // Definir las URLs de las APIs
            string apiPacientes = "https://localhost:44323/api/Pacientes";
            string apiMedicos = "https://localhost:44323/api/Medicos";
            string apiConsultorios = "https://localhost:44323/api/Consultorios"; // URL para consultorios

            // Realizar las solicitudes de forma concurrente
            var responsePacientesTask = _httpClient.GetStringAsync(apiPacientes);
            var responseMedicosTask = _httpClient.GetStringAsync(apiMedicos);
            var responseConsultoriosTask = _httpClient.GetStringAsync(apiConsultorios); // Solicitud para consultorios

            // Esperar las respuestas de todas las APIs
            await Task.WhenAll(responsePacientesTask, responseMedicosTask, responseConsultoriosTask);

            // Deserializar los resultados
            var pacientes = JsonConvert.DeserializeObject<List<Paciente>>(responsePacientesTask.Result);
            var medicos = JsonConvert.DeserializeObject<List<Medico>>(responseMedicosTask.Result);
            var consultorios = JsonConvert.DeserializeObject<List<Consultorio>>(responseConsultoriosTask.Result); // Deserializar los consultorios

            // Pasar los datos a la vista
            ViewBag.Paciente = pacientes;
            ViewBag.Medico = medicos;
            ViewBag.Consultorio = consultorios; // Pasar consultorios a la vista


            return View(new Cita());
        }

        [HttpPost]
        public async Task<ActionResult> Crear(Cita cita)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(cita), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");  // Redirige a la lista de personal si la creación es exitosa
                }

                return View("Error");  // Si ocurre un error, muestra una página de error
            }

            return View(cita);  // Si el modelo no es válido, permanece en la vista de Crear
        }

        public async Task<ActionResult> Guardar(Cita cita)
        {
            if (ModelState.IsValid)  // Verifica si el modelo es válido
            {

                var content = new StringContent(JsonConvert.SerializeObject(cita), Encoding.UTF8, "application/json");
                // Enviar los datos del departamento a la API para ser guardados en la base de datos
                var response = await _httpClient.PostAsync("", content);
                if (response.IsSuccessStatusCode)  // Si la creación fue exitosa
                {
                    return RedirectToAction("Index");  // Redirige a la lista de departamentos
                }
                return View("Error");  // Si ocurre un error, muestra una página de error
            }
            return View(cita);
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