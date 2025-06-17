
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CitasMedicasFront.Models
{
    public class NotasConsulta
    {
        public int NotaId { get; set; }
        public int CitaId { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public string Sintomas { get; set; }
        public string Observaciones { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public string Medicamentos { get; set; }
    }
}