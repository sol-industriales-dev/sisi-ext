using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_PermisosAutorizaCorreo
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public int permiso { get; set; }
        public bool estatus { get; set; }
    }
}
