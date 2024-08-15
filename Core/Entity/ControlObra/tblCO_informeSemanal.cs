using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Multiempresa;
using Newtonsoft.Json;

namespace Core.Entity.ControlObra
{
    public class tblCO_informeSemanal
    {
        public int id { get; set; }
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public string periodo { get; set; }
        public bool estatus { get; set; }

        public int plantilla_id { get; set; }
        public virtual tblCO_PlantillaInforme plantilla { get; set; }

        public virtual List<tblCO_informeSemanal_detalle> informeDetalles { get; set; }
    }
}
