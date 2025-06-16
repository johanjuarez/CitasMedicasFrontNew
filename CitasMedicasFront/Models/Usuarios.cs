using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CitasMedicasFront.Models
{
    public class Usuarios
    {
        public int UsuarioId { get; set; } 
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public int RolId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int? PersonalId { get; set; }
        public string Correo { get; set; }
        public String RutaImagen { get; set; }
    }

}
