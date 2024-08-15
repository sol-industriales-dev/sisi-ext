using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Revision_AC
    {
        public int id { get; set; }
        public int equipoID { get; set; }
        public decimal tonelaje { get; set; }
        public int area { get; set; }
        public int periodo { get; set; }
        public int tecnico { get; set; }
        public int ayudantes { get; set; }
        public string observaciones { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool estatus { get; set; }

        public int planMesDetalleID { get; set; }
    }
}
