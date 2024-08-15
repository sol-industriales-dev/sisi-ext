using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado
{
    public class tblC_SaldoConciliado
    {
        public int id { get; set; }
        public string cc { get; set; }
        public decimal saldo { get; set; }
        public DateTime fecha { get; set; }
        public bool esActivo { get; set; }
        public DateTime ultimoCambio { get; set; }
    }
}
