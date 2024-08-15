using Core.Entity.Principal.Usuarios;
using System;

namespace Core.Entity.GestorArchivos
{
    public class tblGA_Version
    {
        public int id { get; set; }
        public int directorioID { get; set; }
        public virtual tblGA_Directorio directorio { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public int version { get; set; }
        public string ruta { get; set; }
        public string nombre { get; set; }
        public DateTime fecha { get; set; }
        public bool esActivo { get; set; }
    }
}
