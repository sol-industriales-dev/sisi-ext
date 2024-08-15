using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas.Menus
{
    public class tblP_Permisos
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string permiso { get; set; }
        public bool tiene { get; set; }

    }
}
