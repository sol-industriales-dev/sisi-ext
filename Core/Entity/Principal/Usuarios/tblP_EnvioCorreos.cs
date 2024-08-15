using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_EnvioCorreos
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public int moduloID { get; set; }
        public bool estatus { get; set; }
        public bool centroCostosPermiso { get; set; }

    }
}
