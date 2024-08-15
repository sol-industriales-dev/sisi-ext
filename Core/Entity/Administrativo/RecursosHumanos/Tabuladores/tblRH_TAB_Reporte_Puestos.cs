using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Tabuladores
{
    public class tblRH_TAB_Reporte_Puestos
    {
        public int id { get; set; }
        public int FK_Reporte { get; set; }
        public int FK_Puesto { get; set; }
        public bool registroActivo { get; set; }
    }
}
