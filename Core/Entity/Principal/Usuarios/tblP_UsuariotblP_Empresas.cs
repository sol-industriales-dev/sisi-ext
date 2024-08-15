using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_UsuariotblP_Empresas
    {
        public int id { get; set; }
        public int tblP_Usuario_id { get; set; }
        public int tblP_Empresas_id { get; set; }
    }
}
