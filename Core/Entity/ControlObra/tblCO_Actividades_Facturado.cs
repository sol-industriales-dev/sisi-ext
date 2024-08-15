using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_Actividades_Facturado
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }

        public bool autorizado { get; set; }
        public bool estatus { get; set; }

        public int capitulo_id { get; set; }
        public virtual tblCO_Capitulos capitulo { get; set; }

        public virtual List<tblCO_Actividades_Facturado_Detalle> facturado_detalle { get; set; }
    }
}
