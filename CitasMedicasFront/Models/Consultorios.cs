using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitasMedicasFront.Models
{
    public class Consultorio
    {
        public int ConsultorioId { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }

    }
}