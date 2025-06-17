using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CitasMedicasFront.Models.DTOS
{
    public class NotaCrearDto
    {
        public int CitaId { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
    }
}