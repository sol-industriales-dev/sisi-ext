using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class MovProDTO
    {
        public string numpro { get; set; }
        public string factura { get; set; }
        public DateTime fecha { get; set; }
        public int tm { get; set; }
        public string tmDesc { get; set; }
        public DateTime fechavenc { get; set; }
        public string concepto { get; set; }
        public string cc { get; set; }
        public string referenciaoc { get; set; }
        public decimal monto { get; set; }
        public decimal tipocambio { get; set; }
        public decimal iva { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public int linea { get; set; }
        public string generado { get; set; }
        public string es_factura { get; set; }
        public string moneda { get; set; }
        public string autorizapago { get; set; }
        public decimal total { get; set; }
        public int autoincremento { get; set; }
        #region Condensado de saldos
        public decimal saldo { get; set; }
        public decimal vencido { get; set; }
        public DateTime fechaFactura { get; set; }
        public DateTime fechaVence { get; set; }
        public int id { get; set; }
        public bool esPropuesta { get; set; }
        #endregion
    }
}
