using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Poliza
{
    public class tblPo_Poliza
    {
        public int id { get; set; }
        public string poliza { get; set; }
        public string tipoPoliza { get; set; }
        public string estatus { get; set; }
        public string generada { get; set; }
        public DateTime fecha { get; set; }
        public string concepto { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public decimal diferencia { get; set; }
    }
}
