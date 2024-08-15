using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Area
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int cuadrillaID { get; set; }
        public bool estatus { get; set; }
    }
}
