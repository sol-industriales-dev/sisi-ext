using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.Evaluacion
{
    public class tblCO_ADP_EvaluacionRel
    {
        public int id { get; set; }
        public int idSubContratista { get; set; }
        public bool Preguntar { get; set; }
        public int idReq { get; set; }
        public int idAsignacion { get; set; }

    }
}
