using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_PlanMes_Detalle_Dia
    {
        public int id { get; set; }
        public int planMesDetalleID { get; set; }
        public int dia { get; set; }
        public bool estatus { get; set; }
    }
}
