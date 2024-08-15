using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;

namespace Core.Entity.GestorArchivos
{
    public class tblGA_Permisos
    {

        public int id { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public int directorioID { get; set; }
        public virtual tblGA_Directorio directorio { get; set; }
        public bool puedeSubir { get; set; }
        public bool puedeEliminar { get; set; }
        public bool puedeDescargarArchivo { get; set; }
        public bool puedeDescargarCarpeta { get; set; }
        public bool puedeActualizar { get; set; }
        public bool puedeCrear { get; set; }

    }
}
