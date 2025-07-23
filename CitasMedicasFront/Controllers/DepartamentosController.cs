using CitasMedicasFront.Helpers;
using CitasMedicasFront.Models;
using CitasMedicasFront.Models.DTOS; // Aquí debe estar EncryptedDto
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CitasMedicasFront.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly HttpClient _httpClient;

        public DepartamentosController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            _httpClient.BaseAddress = new Uri(ApiUrls.Departamentos);
        }

        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("");
            var encryptedResult = JsonConvert.DeserializeObject<EncryptedDto>(response);

            var jsonPlano = Encriptado.Desencriptar(encryptedResult.Data);
            var departamentos = JsonConvert.DeserializeObject<List<Departamento>>(jsonPlano);

            return View(departamentos);
        }

        public ActionResult Crear()
        {
            return View(new Departamento());
        }

        public async Task<ActionResult> Guardar(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                var jsonPlano = JsonConvert.SerializeObject(departamento);
                var jsonCifrado = Encriptado.Encriptar(jsonPlano);

                var encryptedDto = new EncryptedDto { Data = jsonCifrado };
                var content = new StringContent(JsonConvert.SerializeObject(encryptedDto), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View("Error");
            }

            return View(departamento);
        }

        public async Task<ActionResult> Editar(int id)
        {
            var response = await _httpClient.GetStringAsync($"?id={id}");
            var encryptedResult = JsonConvert.DeserializeObject<EncryptedDto>(response);

            var jsonPlano = Encriptado.Desencriptar(encryptedResult.Data);
            var departamento = JsonConvert.DeserializeObject<Departamento>(jsonPlano);

            if (departamento == null)
                return HttpNotFound();

            return View(departamento);
        }

        [HttpPost]
        public async Task<ActionResult> Actualizar(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                var jsonPlano = JsonConvert.SerializeObject(departamento);
                var jsonCifrado = Encriptado.Encriptar(jsonPlano);

                var encryptedDto = new EncryptedDto { Data = jsonCifrado };
                var content = new StringContent(JsonConvert.SerializeObject(encryptedDto), Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"?id={departamento.DepartamentoId}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View("Error");
            }

            return View(departamento);
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var response = await _httpClient.DeleteAsync($"?id={id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View("Error");
        }
    }
}
