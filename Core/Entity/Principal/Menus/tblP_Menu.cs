using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Menus
{
    public class tblP_Menu
    {
        public int id { get; set; }
        public int sistemaID { get; set; }
        public virtual tblP_Sistema sistema { get; set; }
        public int padre { get; set; }
        public int nivel { get; set; }
        public int tipo { get; set; }
        public string descripcion { get; set; }
        public string url { get; set; }
        public string icono { get; set; }
        public int orden { get; set; }
        public bool visible { get; set; }
        
        public virtual List<tblP_Usuario> usuarios { get; set; }
        public string iconoFont { get; set; }
        public bool activo { get; set; }
        public bool generalPorSistema { get; set; }
        public bool generalSIGOPLAN { get; set; }
        public bool liberado { get; set; }
        public bool desarrollo { get; set; }
        public bool esColombia { get; set; }
        [NotMapped]
        public bool auditable { get; set; }
    }
}
