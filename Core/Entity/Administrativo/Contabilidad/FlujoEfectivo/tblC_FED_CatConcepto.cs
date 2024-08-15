using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FED_CatConcepto
    {
        public int id { get; set; }
        public int idPadre { get; set; }
        public string Concepto { get; set; }
        public string operador { get; set; }
        public int orden { get; set; }
        public int idAccion { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
