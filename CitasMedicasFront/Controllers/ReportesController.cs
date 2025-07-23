using CitasMedicasFront.Helpers;
using CitasMedicasFront.Models.DTOS;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CitasMedicasFront.Controllers
{
    public class ReportesController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReportesController()
        {
            _httpClient = HttpClientInstancia.Instancia;

        }

        public async Task<ActionResult> CitaPdf(int id)
        {
            // Consumir el endpoint que devuelve el DTO con toda la info
            var response = await _httpClient.GetAsync($"{ApiUrls.Reportes}/CitaDetalle/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var json = await response.Content.ReadAsStringAsync();
            var citaDetalle = JsonConvert.DeserializeObject<CitaDetalleDto>(json);

            if (citaDetalle == null)
            {
                return View("Error");
            }

            return new ViewAsPdf("CitaDetallePdf", citaDetalle)
            {
                FileName = $"Cita_{citaDetalle.NombrePaciente}_{citaDetalle.FechaHora:yyyyMMdd}.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }

    }
}
