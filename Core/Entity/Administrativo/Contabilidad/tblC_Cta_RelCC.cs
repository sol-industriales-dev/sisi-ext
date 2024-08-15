using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_Cta_RelCC
    {       
        public int id { get; set; }
        public string ccPrincipal { get; set; }
        public string descripcionCCPrincipal { get; set; }
        public string ccSecundario { get; set; }
        public string descripcionCCSecundario { get; set; }
        public int idUsuarioRegistro { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
