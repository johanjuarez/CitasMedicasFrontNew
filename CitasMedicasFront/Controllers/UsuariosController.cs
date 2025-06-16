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
using System.Web;
using System.Web.Mvc;

namespace CitasMedicasFront.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CatalogosService _catalogosService = new CatalogosService();

        public UsuariosController()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });
            _httpClient.BaseAddress = new Uri(ApiUrls.Usuarios);
        }

        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("");
            var usuarios = JsonConvert.DeserializeObject<List<Usuarios>>(response);
            return View(usuarios);
        }

        public async Task<ActionResult> Crear()
        {
            var personal = await _catalogosService.ObtenerPersonalAsync();
            var roles = await _catalogosService.ObtenerRolesAsync();

            ViewBag.Personal = personal;
            ViewBag.Roles = roles;

            return View(new Usuarios());
        }

        [HttpPost]
        public async Task<ActionResult> Crear(Usuarios usuario)
        {
            var personal = await _catalogosService.ObtenerPersonalAsync();
            var roles = await _catalogosService.ObtenerRolesAsync();
            ViewBag.Personal = personal;
            ViewBag.Roles = roles;

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View("Error");
            }

        

            return View(usuario);
        }

        public async Task<ActionResult> Editar(int id)
        {
            var response = await _httpClient.GetStringAsync($"?id={id}");
            var usuario = JsonConvert.DeserializeObject<Usuarios>(response);

            if (usuario == null)
                return HttpNotFound();

            var personales = await _catalogosService.ObtenerPersonalAsync();
            var roles = await _catalogosService.ObtenerRolesAsync();

            ViewBag.Personal = personales;
            ViewBag.Roles = roles;

            

            return View(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> Actualizar(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"?id={usuario.UsuarioId}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View("Error");
            }

            var personal = await _catalogosService.ObtenerPersonalAsync();
            var roles = await _catalogosService.ObtenerRolesAsync();
            ViewBag.Personal = personal;
            ViewBag.Roles = roles;

            return View("Editar", usuario);
        }

        //public async Task<ActionResult> Guardar(Usuarios usuarios)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var content = new StringContent(JsonConvert.SerializeObject(usuarios), Encoding.UTF8, "application/json");
        //        var response = await _httpClient.PostAsync("", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        return View("Error");
        //    }
        //    return View(usuarios);
        //}

        [HttpPost]
        public async Task<ActionResult> Guardar(Usuarios usuarios, HttpPostedFileBase FotoFile)
        {
            if (ModelState.IsValid)
            {
                // Guardar imagen antes de llamar a la API
                if (FotoFile != null)
                    usuarios.RutaImagen = GuardarImagen(FotoFile, usuarios.Usuario); // o usuarios.Nombre

                var content = new StringContent(JsonConvert.SerializeObject(usuarios), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View("Error");
            }

            return View(usuarios);
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var response = await _httpClient.DeleteAsync($"?id={id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View("Error");
        }

        private string GuardarImagen(HttpPostedFileBase archivo, string nombreUsuario)
        {
            if (archivo == null || string.IsNullOrWhiteSpace(nombreUsuario))
                return null;

            // Carpeta relativa dentro del proyecto (para que IIS Express pueda servir las imágenes)
            string carpetaRelativa = "~/ImagenesUsuarios/";
            string rutaRelativa = carpetaRelativa + nombreUsuario + Path.GetExtension(archivo.FileName);

            // Mapeamos ruta relativa a ruta física dentro del proyecto
            string rutaFisica = Server.MapPath(rutaRelativa);

            // Creamos la carpeta si no existe
            Directory.CreateDirectory(Path.GetDirectoryName(rutaFisica));

            // Guardamos el archivo físicamente en la ruta mapeada
            archivo.SaveAs(rutaFisica);

            // Retornamos la ruta para usar en el navegador (URL relativa)
            return Url.Content(rutaRelativa);
        }

    }
}
