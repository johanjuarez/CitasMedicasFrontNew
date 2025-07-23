using CitasMedicasFront.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CitasMedicasFront.Models.DTOS;

namespace CitasMedicasFront.Helpers
{
    public class CatalogosService
    {
        private readonly HttpClient _httpClient = HttpClientProvider.Client;

        public async Task<List<Departamento>> ObtenerDepartamentosAsync()
        {
            var json = await _httpClient.GetStringAsync(ApiUrls.Departamentos);
            var encryptedDto = JsonConvert.DeserializeObject<EncryptedDto>(json);
            var jsonDesencriptado = Encriptado.Desencriptar(encryptedDto.Data);

            return JsonConvert.DeserializeObject<List<Departamento>>(jsonDesencriptado);
        }

        public async Task<List<Paciente>> ObtenerPacientesAsync()
        {
            var response = await _httpClient.GetStringAsync(ApiUrls.Pacientes);
            return JsonConvert.DeserializeObject<List<Paciente>>(response);
        }

        public async Task<List<Medico>> ObtenerMedicosAsync()
        {
            var response = await _httpClient.GetStringAsync(ApiUrls.Medicos);
            return JsonConvert.DeserializeObject<List<Medico>>(response);
        }

        public async Task<List<Consultorio>> ObtenerConsultoriosAsync()
        {
            var response = await _httpClient.GetStringAsync(ApiUrls.Consultorios);
            return JsonConvert.DeserializeObject<List<Consultorio>>(response);
        }

        public async Task<List<EstatusCita>> ObtenerEstatusCitaAsync()
        {
            var response = await _httpClient.GetStringAsync(ApiUrls.EstatusCita);
            return JsonConvert.DeserializeObject<List<EstatusCita>>(response);
        }
        public async Task<List<Personal>> ObtenerPersonalAsync()
        {
            var response = await _httpClient.GetStringAsync(ApiUrls.Personal);
            return JsonConvert.DeserializeObject<List<Personal>>(response);
        }
        public async Task<List<Rol>> ObtenerRolesAsync()
        {
            var response = await _httpClient.GetStringAsync(ApiUrls.Roles);
            return JsonConvert.DeserializeObject<List<Rol>>(response);
        }
    }
}
