using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using CitasMedicasFront.Models;
using Newtonsoft.Json;
using System.Text;

namespace CitasMedicasFront.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly string apiUrl = "https://localhost:44323/api/Usuarios";

        // GET: Usuarios
        public async Task<ActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var usuarios = JsonConvert.DeserializeObject<List<Usuarios>>(data);
                    return View(usuarios);
                }
                return View(new List<Usuarios>());
            }
        }

        // GET: Usuarios/Create
        public ActionResult Crear()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        public async Task<ActionResult> Crear(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(usuario);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var usuario = JsonConvert.DeserializeObject<Usuarios>(data);
                    return View(usuario);
                }
            }
            return HttpNotFound();
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(usuario);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync($"{apiUrl}/{usuario.UsuarioId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var usuario = JsonConvert.DeserializeObject<Usuarios>(data);
                    return View(usuario);
                }
            }
            return HttpNotFound();
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync($"{apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}
