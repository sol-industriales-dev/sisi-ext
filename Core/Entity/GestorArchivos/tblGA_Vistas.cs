using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;

namespace Core.Entity.GestorArchivos
{
    public class tblGA_Vistas
    {

        public int id { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public int directorioID { get; set; }
        public virtual tblGA_Directorio directorio { get; set; }
        public int estatusVista { get; set; } //0 Deseleccionada  - 1 Parcialmente Seleccionada - 2 Seleccionada

    }
}
