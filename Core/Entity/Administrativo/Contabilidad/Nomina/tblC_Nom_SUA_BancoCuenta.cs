using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_SUA_BancoCuenta
    {
        public int id { get; set; }
        public string estado { get; set; }
        public string banco { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
        public bool registroActivo { get; set; }
    }
}
