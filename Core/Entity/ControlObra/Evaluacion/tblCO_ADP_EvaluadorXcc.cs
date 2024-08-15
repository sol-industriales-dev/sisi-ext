using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.Evaluacion
{
    public class tblCO_ADP_EvaluadorXcc
    {
        public int id { get; set; }
        public int evaluador { get; set; }
        public string cc { get; set; }
        public string elementos { get; set; }
        public bool esActivo { get; set; }
    }
}
