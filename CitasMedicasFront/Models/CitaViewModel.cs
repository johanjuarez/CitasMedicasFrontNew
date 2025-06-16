using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitasMedicasFront.Models
{
    public class CitaViewModel
    {
        public int Id { get; set; }
        public string PacienteNombre { get; set; }
        public string MedicoNombre { get; set; }
        public string ConsultorioNombre { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }
        public string Motivo { get; set; }
        public string Notas { get; set; }
    }
}