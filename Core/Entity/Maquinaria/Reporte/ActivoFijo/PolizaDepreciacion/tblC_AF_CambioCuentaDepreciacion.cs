using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_CambioCuentaDepreciacion
    {
        public int id { get; set; }
        public int maquinaId { get; set; }
        public string poliza { get; set; }
        public int cuentaCargo { get; set; }
        public int subcuentaCargo { get; set; }
        public int subsubcuentaCargo { get; set; }
        public int cuentaDep { get; set; }
        public int subcuentaDep { get; set; }
        public int subsubcuentaDep { get; set; }
        public DateTime fechaAplica { get; set; }
        public bool registroActivo { get; set; }
    }
}
