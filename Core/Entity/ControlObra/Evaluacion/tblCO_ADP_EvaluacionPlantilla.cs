using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.Evaluacion
{
    public class tblCO_ADP_EvaluacionPlantilla
    {
        public int id { get; set; }
        public string nombrePlantilla { get; set; }
        public bool esActivo { get; set; }
        public string contratos { get; set; }
    }
}
