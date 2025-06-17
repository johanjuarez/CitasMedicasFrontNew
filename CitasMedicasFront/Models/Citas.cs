using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitasMedicasFront.Models
{
    public class Cita
    {
        public int CitaId { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int ConsultorioId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
        public int Estatus { get; set; } 
        public string Notas { get; set; }
        public int UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaHoraFin { get; set; }

        public DateTime FechaUltimaModificacion { get; set; }
        public int  UsuarioUltimaModificacion { get; set; }

        public virtual Paciente Paciente { get; set; }
        public virtual Medico Medico { get; set; }
        public virtual Consultorio Consultorio { get; set; }


    }
}