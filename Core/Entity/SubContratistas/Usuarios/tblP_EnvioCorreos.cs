using Core.Entity.SubContratistas.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas.Usuarios
{
    public class tblP_EnvioCorreos
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuarios usuario { get; set; }
        public int moduloID { get; set; }
        public bool estatus { get; set; }
        public bool centroCostosPermiso { get; set; }

    }
}
