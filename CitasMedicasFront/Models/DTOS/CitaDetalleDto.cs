using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CitasMedicasFront.Models.DTOS
{
    public class CitaDetalleDto
    {
        public int CitaId { get; set; }

        // Datos del paciente
        public string NombrePaciente { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        // Datos del médico
        public string NombreMedico { get; set; }
        public string Especialidad { get; set; }

        // Datos de la cita
        public DateTime FechaHora { get; set; }
        public DateTime? FechaHoraFin { get; set; }
        public string Motivo { get; set; }
        public string Notas { get; set; }

        // Datos del consultorio
        public string NombreConsultorio { get; set; }
    }

}