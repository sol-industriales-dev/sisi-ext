using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_Actividades_Facturado_Detalle
    {
        public int id { get; set; }      
        public decimal volumen { get; set; }
        public decimal importe { get; set; }
        public bool estatus { get; set; }

        public int actividad_id { get; set; }
        public virtual tblCO_Actividades actividad { get; set; }

        public int? actividadFacturado_id { get; set; }
        public virtual tblCO_Actividades_Facturado facturado { get; set; }
    }
}
