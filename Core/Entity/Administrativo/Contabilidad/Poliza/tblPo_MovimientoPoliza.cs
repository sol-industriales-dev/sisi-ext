using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Poliza
{
    public class tblPo_MovimientoPoliza
    {
        public int id { get; set; }
        public int idPoliza { get; set; }
        public int linea { get; set; }
        public string cta { get; set; }
        public string scta { get; set; }
        public string sscta { get; set; }
        public int digito { get; set; }
        public string movimiento { get; set; }
        public string numProverdor { get; set; }
        public string referencia { get; set; }
        public string orderCompraCliente { get; set; }
        public string concepto { get; set; }
        public string tipoMovimiento { get; set; }
        public decimal Monto { get; set; }
        public int tipoMoneda { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
    }
}
