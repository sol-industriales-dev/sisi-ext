using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_PlanMes
    {
        public int id { get; set; }
        public int cuadrillaID { get; set; }
        public int periodo { get; set; }
        public int mes { get; set; }
        public int anio { get; set; }
        public bool estatus { get; set; }
    }
}
