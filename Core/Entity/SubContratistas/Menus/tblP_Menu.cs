using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas.Menus
{
    public class tblP_Menu
    {
        public int id { get; set; }
        public int sistemaID { get; set; }
        public int padre { get; set; }
        public int nivel { get; set; }
        public int tipo { get; set; }
        public string descripcion { get; set; }
        public string url { get; set; }
        public string icono { get; set; }
        public int orden { get; set; }
        public bool visible { get; set; }
        public string iconoFont { get; set; }
        public bool activo { get; set; }
        public bool liberado { get; set; }
        public bool desarrollo { get; set; }
        public int perfil { get; set; }
    }
}
