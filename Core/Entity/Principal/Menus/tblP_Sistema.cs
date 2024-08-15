using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Menus
{
    public class tblP_Sistema
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string icono { get; set; }
        public string url { get; set; }
        public bool estatus { get; set; }
        public bool activo { get; set; }
        public bool general { get; set; }
        //raguilar 091117 propiedad para indicar modulos externos al proyecto "procedimientos"
        public bool ext { get; set; }
        public bool esColombia { get; set; }
        public bool esVirtual { get; set; }
    }
}
