using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Poliza
{
    public class tblC_NominaPoliza
    {
        public int id { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public DateTime fecha { get; set; }
        public int tipoNomina { get; set; }
        public int tipoCuenta { get; set; }
        public string cc { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public decimal iva { get; set; }
        public decimal retencion { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCaptura { get; set; }
    }
}
