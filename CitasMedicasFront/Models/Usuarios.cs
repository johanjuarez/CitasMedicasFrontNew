using System;

namespace CitasMedicasFront.Models
{
    public class Usuarios
    {
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Folio { get; set; }
        public string Contraseña { get; set; }
        public int RolId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int? PersonalId { get; set; }
    }
}
