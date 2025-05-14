using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using CitasMedicasFront.Models;  // Asegúrate de usar el espacio de nombres correcto

namespace CitasMedicasFront.Controllers
{
    public class PacientesController : Controller
    {
        private readonly HttpClient _httpClient;

        public PacientesController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                // Ignora la validación del certificado SSL
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            _httpClient.BaseAddress = new Uri("https://localhost:44323/api/Pacientes");
        }

        // GET: Pacientes
        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync(""); // Obtén todos los pacientes
            var pacientes = JsonConvert.DeserializeObject<List<Paciente>>(response);

            return View(pacientes);  // Devuelve la vista con los pacientes
        }

        // GET: Pacientes/Crear
        public ActionResult Crear()
        {
            return View(new Paciente());  // Crea una nueva instancia de Paciente para la vista
        }

        //// POST: Pacientes/Crear
        ////[HttpPost]
        public async Task<ActionResult> Guardar(Paciente paciente)
        {
            if (ModelState.IsValid)  // Verifica si el modelo es válido
            {
                var content = new StringContent(JsonConvert.SerializeObject(paciente), Encoding.UTF8, "application/json");

                // Enviar los datos del paciente a la API para ser guardados en la base de datos
                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)  // Si la creación fue exitosa
                {
                    return RedirectToAction("Index");  // Redirige a la lista de pacientes
                }

                return View("Error");  // Si ocurre un error, muestra una página de error
            }

            return View(paciente);  // Si el modelo no es válido, permanece en la vista de Crear
        }

        // GET: Pacientes/Editar/5
        public async Task<ActionResult> Editar(int id)
        {
            var response = await _httpClient.GetStringAsync($"?id={id}");
            var paciente = JsonConvert.DeserializeObject<Paciente>(response);

            if (paciente == null)
            {
                return HttpNotFound();  // Si no se encuentra el paciente, se devuelve un error 404
            }

            return View(paciente);  // Devuelve la vista con los datos del paciente
        }

        // POST: Pacientes/Actualizar
        [HttpPost]
        public async Task<ActionResult> Actualizar(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(paciente), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"?id={paciente.PacienteId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");  // Redirige a la lista de pacientes si se actualizó correctamente
                }

                return View("Error");  // En caso de error
            }

            return View(paciente);  // Si el modelo no es válido, permanece en la vista actual
        }

        //// POST: Pacientes/Eliminar
        //[HttpPost]
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
