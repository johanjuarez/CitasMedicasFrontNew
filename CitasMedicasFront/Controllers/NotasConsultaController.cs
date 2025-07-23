
using CitasMedicasFront.Helpers;
using CitasMedicasFront.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CitasMedicasFront.Controllers
{
    public class NotasConsultaController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CatalogosService _catalogosService = new CatalogosService();
        public NotasConsultaController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });
            _httpClient.BaseAddress = new Uri(ApiUrls.NotasConsulta);
        }
        public ActionResult Crear(int citaId, int medicoId, int pacienteId)
        {
            var nota = new NotasConsulta
            {
                CitaId = citaId,
                MedicoId = medicoId,
                PacienteId = pacienteId
            };

            return View(nota); 
        }

        public async Task<ActionResult> Guardar(NotasConsulta notasConsulta)
        {
            if (ModelState.IsValid)
            {
                // Enviar la nota a la API
                var content = new StringContent(JsonConvert.SerializeObject(notasConsulta), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    // Obtener el ID de la nota recién creada
                    var responseString = await response.Content.ReadAsStringAsync();
                    var notaCreada = JsonConvert.DeserializeObject<NotasConsulta>(responseString);
                    int notaId = notaCreada.NotaId;
      
                    // Enviar una solicitud para asignar el NotaId a la Cita correspondiente
                    var asignarNotaResponse = await _httpClient.PutAsync($"{ApiUrls.Citas}/AsignarNota/{notasConsulta.CitaId}/{notaId}", null);

                    if (!asignarNotaResponse.IsSuccessStatusCode)
                    {
                        return View("Error"); // Error al asignar el ID  de la nota a la cita 
                    }

                    return RedirectToAction("CitasMedicos", "Citas");
                }

                return View("Error"); // Falló al guardar la nota
            }

            return View(notasConsulta);
        }



    }
}