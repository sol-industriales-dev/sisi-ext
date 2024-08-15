using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FE_CatConcepto
    {
        public int id { get; set; }
        public int idpadre { get; set; }
        public string Concepto { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
