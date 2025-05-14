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
    public class ConsultoriosController : Controller
    {
        private readonly HttpClient _httpClient;

        public ConsultoriosController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });
            _httpClient.BaseAddress = new Uri("https://localhost:44323/api/Consultorios");
        }

        // GET: Consultorios
        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync(""); // Obtén todos los departamentos
            var consultorios = JsonConvert.DeserializeObject<List<Consultorio>>(response);

            return View(consultorios);  // Devuelve la vista con los pacientes
        }
        public ActionResult Crear()
        {
            return View(new Consultorio());  // Crea una nueva instancia de Departamentos para la vista
        }
        public async Task<ActionResult> Guardar(Consultorio consultorio)
        {
            if (ModelState.IsValid)  // Verifica si el modelo es válido
            {

                var content = new StringContent(JsonConvert.SerializeObject(consultorio), Encoding.UTF8, "application/json");
                // Enviar los datos del departamento a la API para ser guardados en la base de datos
                var response = await _httpClient.PostAsync("", content);
                if (response.IsSuccessStatusCode)  // Si la creación fue exitosa
                {
                    return RedirectToAction("Index");  // Redirige a la lista de departamentos
                }
                return View("Error");  // Si ocurre un error, muestra una página de error
            }
            return View(consultorio);
        }

        public async Task<ActionResult> Editar(int id)
        {
            var response = await _httpClient.GetStringAsync($"?id={id}");
            var consultorio = JsonConvert.DeserializeObject<Consultorio>(response);

            if (consultorio == null)
            {
                return HttpNotFound();  // Si no se encuentra el paciente, se devuelve un error 404
            }

            return View(consultorio);  // Devuelve la vista con los datos del paciente
        }

        [HttpPost]
        public async Task<ActionResult> Actualizar(Consultorio consultorio)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(consultorio), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"?id={consultorio.ConsultorioId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");  // Redirige a la lista de pacientes si se actualizó correctamente
                }

                return View("Error");  // En caso de error
            }

            return View(consultorio);  // Si el modelo no es válido, permanece en la vista actual
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