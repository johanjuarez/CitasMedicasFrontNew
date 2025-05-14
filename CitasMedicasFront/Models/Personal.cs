using System;

namespace CitasMedicasFront.Models
{
    public class Personal
    {
        public int PersonalId { get; set; }  // Identificador único del personal
        public string Nombre { get; set; }  // Nombre del personal
        public string ApellidoPaterno { get; set; }  // Apellido paterno
        public string ApellidoMaterno { get; set; }  // Apellido materno (opcional)
        public string Correo { get; set; }  // Correo electrónico
        public string Telefono { get; set; }  // Teléfono de contacto
        public string Direccion { get; set; }  // Dirección de residencia
        public DateTime? FechaNacimiento { get; set; }  // Fecha de nacimiento (opcional)
        public string RFC { get; set; }  // Registro Federal de Contribuyentes (opcional)
        public string NSS { get; set; }  // Número de Seguro Social (opcional)
        public string Cargo { get; set; }  // Cargo o puesto que desempeña
        public DateTime FechaContratacion { get; set; }  // Fecha de contratación
        public DateTime? FechaRegistro { get; set; }  // Fecha de registro en el sistema (opcional)
        public int? UsuarioId { get; set; }  // ID del usuario relacionado (opcional)
        public string Turno { get; set; }  // Turno de trabajo (opcional)
        public int DepartamentoId { get; set; }  // ID del departamento al que pertenece
        public string Sexo { get; set; }  // Sexo del personal (opcional)
        public string CURP { get; set; }  // Clave Única de Registro de Población (opcional)
    }
}
