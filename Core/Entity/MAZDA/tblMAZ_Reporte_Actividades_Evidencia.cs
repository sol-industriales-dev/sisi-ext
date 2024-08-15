using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Reporte_Actividades_Evidencia
    {
        public int id { get; set; }
        public int reporteActividadesID { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
        public bool estatus { get; set; }
    }
}
