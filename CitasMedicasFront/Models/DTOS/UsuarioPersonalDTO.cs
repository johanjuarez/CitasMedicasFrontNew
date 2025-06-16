
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CitasMedicasFront.Models.DTOS
{
    public class UsuarioPersonalDTO
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string ImagenPerfil { get; set; }
        public int RolUsuario { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string NombreReal { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}