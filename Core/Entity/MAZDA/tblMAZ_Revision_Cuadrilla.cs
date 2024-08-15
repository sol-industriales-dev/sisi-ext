using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Revision_Cuadrilla
    {
        public int id { get; set; }
        public int cuadrillaID { get; set; }
        public int mes { get; set; }
        public int tecnico { get; set; }
        public string observaciones { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool estatus { get; set; }

        public int planMesDetalleID { get; set; }
    }
}
