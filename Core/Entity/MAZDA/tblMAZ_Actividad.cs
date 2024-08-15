using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Actividad
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string detalle { get; set; }
        public int areaID { get; set; }
        public int periodo { get; set; }
        public bool estatus { get; set; }
    }
}
