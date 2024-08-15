using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Reporte_Actividades_Equipo
    {
        public int id { get; set; }
        public int reporteActividadesID { get; set; }
        public int equipoID { get; set; }
        public bool estatus { get; set; }
    }
}
