using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Configuraciones
{
    public class tblP_ReglasSubcontratistasBloqueo
    {
        public int id { get; set; }
        public string regla { get; set; }
        public bool aplicar { get; set; }
        public bool estatus { get; set; }
    }
}
