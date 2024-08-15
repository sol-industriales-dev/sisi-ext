using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FED_SaldoInicial
    {
        public int id { get; set; }
        public string cc { get; set; }
        public decimal saldo { get; set; }
        public int anio { get; set; }
        public int idConceptoDir { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
