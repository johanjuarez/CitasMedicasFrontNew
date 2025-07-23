using CitasMedicasFront.Helpers;
using CitasMedicasFront.Models;
using CitasMedicasFront.Models.DTOS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CitasMedicasFront.Controllers
{
    public class CitasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CatalogosService _catalogosService = new CatalogosService();

        public CitasController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            _httpClient.BaseAddress = new Uri(ApiUrls.Citas); 
        }

        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("");
            var citas = JsonConvert.DeserializeObject<List<Cita>>(response);
            return View(citas);
        }

        public async Task<ActionResult> CitasMedicos()
        {
            int usuarioId = (int)Session["UserId"];

            try
            {
                var response = await _httpClient.GetAsync($"{ApiUrls.Citas}/Medico/{usuarioId}");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "No se pudieron obtener las citas.";
                    return View(new List<Cita>());
                }

                var json = await response.Content.ReadAsStringAsync();
                var citas = JsonConvert.DeserializeObject<List<Cita>>(json);

                return View(citas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al conectar con el servidor: " + ex.Message;
                return View(new List<Cita>());
            }
        }

        public async Task<ActionResult> Crear()
        {
            var pacientes = await _catalogosService.ObtenerPacientesAsync();
            var medicos = await _catalogosService.ObtenerMedicosAsync();
            var consultorios = await _catalogosService.ObtenerConsultoriosAsync();
            var estatusCita = await _catalogosService.ObtenerEstatusCitaAsync();

            ViewBag.Paciente = pacientes;
            ViewBag.Medico = medicos;
            ViewBag.Consultorio = consultorios;
            ViewBag.EstatusCita = estatusCita;

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
                    return RedirectToAction("Index");
                }

                return View("Error");
            }

            return View(cita);
        }

        public async Task<ActionResult> Guardar(Cita cita)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(cita), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
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

        public ActionResult Calendario()
        {
            return View();
        }
    }
}
