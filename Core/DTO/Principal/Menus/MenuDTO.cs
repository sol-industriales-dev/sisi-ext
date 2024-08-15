using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Menus
{
    public class MenuDTO
    {
        public int id { get; set; }
        public int padre { get; set; }
        public int nivel { get; set; }
        public int orden { get; set; }
        public string text { get; set; }
        public string state { get; set; }
        public int tipo { get; set; }
        public string action { get; set; }
        public string url { get; set; }
        public string icono { get; set; }
        public List<MenuDTO> children { get; set; }
        public string iconoFont { get; set; }
        public bool activo { get; set; }
        public bool general { get; set; }
        public bool liberado { get; set; }
        public bool desarrollo { get; set; }
        public bool auditable { get; set; }

    }
}
