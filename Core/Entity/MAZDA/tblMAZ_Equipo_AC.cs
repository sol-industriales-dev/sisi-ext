using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Equipo_AC
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string caracteristicas { get; set; }
        public string modelo { get; set; }
        public string tonelaje { get; set; }
        public int subAreaID { get; set; }
        public string subArea { get; set; }
        public Int32? cantidad { get; set; }
        public bool estatus { get; set; }
    }
}
