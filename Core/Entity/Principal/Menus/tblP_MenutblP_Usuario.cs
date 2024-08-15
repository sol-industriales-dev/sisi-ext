using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Menus
{
    public class tblP_MenutblP_Usuario
    {
        public int id { get; set; }
        public int tblP_Usuario_id { get; set; }
        public int tblP_Menu_id { get; set; }
        public int sistema { get; set; }
    }
}
