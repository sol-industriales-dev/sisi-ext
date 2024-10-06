using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_TipoMoneda
    {
        public int id { get; set; }
        public string nombreCorto { get; set; }
        public string nombreCortoSat { get; set; }
        public bool esMXN { get; set; }
        public bool registroActivo { get; set; }
        public string signo { get; set; }
    }
}
