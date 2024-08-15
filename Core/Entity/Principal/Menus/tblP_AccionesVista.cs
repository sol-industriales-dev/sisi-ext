using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Menus
{
    public class tblP_AccionesVista
    {
        public int id { get; set; }
        public int vistaID { get; set; }
        //        public virtual tblP_Menu vista { get; set; }
        public string Accion { get; set; }
        public virtual List<tblP_Usuario> usuarios { get; set; }

        public string Descripcion { get; set; }
        
    }
}
