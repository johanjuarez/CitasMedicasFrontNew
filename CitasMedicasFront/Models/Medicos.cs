using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitasMedicasFront.Models
{
    public class Medico
    {
        public int MedicoId { get; set; }
        public int PersonalId { get; set; }
        public string Especialidad { get; set; }
        public string CedulaProfesional { get; set; }
        public int ConsultorioId { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }

        // Relación con la tabla Personal
        public virtual Personal Personal { get; set; }

    }
}