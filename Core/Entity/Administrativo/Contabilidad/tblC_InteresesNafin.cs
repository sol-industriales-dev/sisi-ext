using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_InteresesNafin
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string banco { get; set; }
        public decimal totalCadenas { get; set; }
        public decimal totalBanco { get; set; }
        public decimal interes { get; set; }
        public decimal tipoCambio { get; set; }
        public int divisa { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCaptura { get; set; }
    }
}
