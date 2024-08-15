using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_ADP_EvaluacionDiv
    {
        public int id { get; set; }
        public string idbutton { get; set; }
        public string idsection { get; set; }
        public int orden { get; set; }
        public string toltips { get; set; }
        public string descripcion { get; set; }
        public bool esActivo { get; set; }
        public bool SubContratista { get; set; }
        public bool important { get; set; }
        public int idEvaluador { get; set; }
        public int idPlantilla { get; set; }
    }
}
