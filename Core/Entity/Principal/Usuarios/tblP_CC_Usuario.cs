using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_CC_Usuario
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public string cc { get; set; }
        public bool reasignado { get; set; }
    }
}
