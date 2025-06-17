
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

        //public async Task<ActionResult> Editar(int? id)
        //{
        //    var response = await _httpClient.GetStringAsync($"?id={id}");
        //    var nota = JsonConvert.DeserializeObject<NotasConsulta>(response);
        //    if (nota == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(nota);
        //}
        public async Task<ActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var response = await _httpClient.GetAsync($"/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var json = await response.Content.ReadAsStringAsync();
            var nota = JsonConvert.DeserializeObject<NotasConsulta>(json);

            if (nota == null)
            {
                return HttpNotFound();
            }

            return View(nota);
        }



        public async Task<ActionResult> Actualizar(NotasConsulta notasConsulta)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(notasConsulta), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"?id={notasConsulta.NotaId}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }
            return View("Editar", notasConsulta);
        }

    } 
}