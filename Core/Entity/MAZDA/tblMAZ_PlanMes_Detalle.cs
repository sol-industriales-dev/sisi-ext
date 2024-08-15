using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_PlanMes_Detalle
    {
        public int id { get; set; }
        public int planMesID { get; set; }
        public int tipo { get; set; }
        public int equipoID { get; set; }
        public bool estatus { get; set; }
    }
}
