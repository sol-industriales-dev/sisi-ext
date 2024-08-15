using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Menus
{
    public class tblP_MenuUsuarioBloqueado
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public int menuId { get; set; }
        public bool registroActivo { get; set; }
    }
}
