using System;

namespace CitasMedicasFront.Models
{
    public class Paciente
    {
        public int PacienteId { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        public string Direccion { get; set; }
        public string NSS { get; set; }
        public string Alergias { get; set; }
        public string EnfermedadesCronicas { get; set; }
        public string Discapacidades { get; set; }
        public DateTime FechaRegistro { get; set; }  // Fecha de contratación
        public DateTime FechaUltimaModificacion { get; set; }


    }
}
