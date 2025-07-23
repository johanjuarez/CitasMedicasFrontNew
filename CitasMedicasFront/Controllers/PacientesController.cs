using CitasMedicasFront.Helpers;
using CitasMedicasFront.Models;
using ClosedXML.Excel;
using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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

            _httpClient.BaseAddress = new Uri(ApiUrls.Pacientes);
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


        private (bool IsValid, string ErrorMessage, Paciente Paciente) ValidarFila(DataRow row, int fila)
        {
            string nombre = row[0]?.ToString().Trim();
            string apellidoP = row[1]?.ToString().Trim();
            string apellidoM = row[2]?.ToString().Trim();
            string fechaNacStr = row[3]?.ToString().Trim();
            string sexo = row[4]?.ToString().Trim();
            string correo = row[5]?.ToString().Trim();
            string telefono = row[6]?.ToString().Trim();
            string fechaRegStr = row[7]?.ToString().Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellidoP) ||
                string.IsNullOrWhiteSpace(apellidoM) || string.IsNullOrWhiteSpace(sexo) ||
                string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(telefono) ||
                string.IsNullOrWhiteSpace(fechaNacStr) || string.IsNullOrWhiteSpace(fechaRegStr))
            {
                return (false, $"Error: Campos vacíos en fila {fila + 2}. No se guardó nada.", null);
            }

            if (!DateTime.TryParse(fechaNacStr, out DateTime fechaNac))
                return (false, $"Error: Fecha de nacimiento inválida en fila {fila + 2}.", null);

            if (!DateTime.TryParse(fechaRegStr, out DateTime fechaReg))
                return (false, $"Error: Fecha de registro inválida en fila {fila + 2}.", null);

            var paciente = new Paciente
            {
                Nombre = nombre,
                ApellidoPaterno = apellidoP,
                ApellidoMaterno = apellidoM,
                FechaNacimiento = fechaNac,
                Sexo = sexo,
                Correo = correo,
                Telefono = telefono,
                FechaRegistro = fechaReg,
                FechaUltimaModificacion = DateTime.Now
            };

            return (true, string.Empty, paciente);
        }


        public async Task<ActionResult> ImportarExcel(HttpPostedFileBase archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.ContentLength == 0)
            {
                ModelState.AddModelError("", "Selecciona un archivo Excel.");
                return View();
            }

            var extension = Path.GetExtension(archivoExcel.FileName);
            if (extension == null || !extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "El archivo debe ser de tipo .xlsx");
                return View();
            }

            var pacientes = new List<Paciente>();

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var stream = archivoExcel.InputStream)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var config = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };

                    var dataSet = reader.AsDataSet(config);
                    var dataTable = dataSet.Tables[0];

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        var row = dataTable.Rows[i];

                        var resultado = ValidarFila(row, i);
                        if (!resultado.IsValid)
                        {
                            ModelState.AddModelError("", resultado.ErrorMessage);
                            return View();
                        }

                        pacientes.Add(resultado.Paciente);
                    }
                }

                var json = JsonConvert.SerializeObject(pacientes);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{ApiUrls.Pacientes}/Importar", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error al guardar los pacientes en la API.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al procesar el archivo: " + ex.Message);
            }

            return View();
        }



    }
}
