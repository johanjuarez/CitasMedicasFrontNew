using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitasMedicasFront.Models
{
    public class Departamento
    {
        public int DepartamentoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}