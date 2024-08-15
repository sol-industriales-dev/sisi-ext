using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class CondSaldoDTO
    {
        public int id { get; set; }
        public int idGiro { get; set; }
        public string numpro { get; set; }
        public string factura { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechavenc { get; set; }
        public string cc { get; set; }
        public decimal tipocambio { get; set; }
        public decimal saldos_factura_dlls { get; set; }
        public decimal saldos_factura { get; set; }
        public decimal total { get; set; }
        public int moneda { get; set; }
        public string descPadre { get; set; }
        public string descGiro { get; set; }
        public string descProv { get; set; }
        public string descCc { get; set; }
        #region Vencimiento
        public decimal saldo { get; set; }
        public decimal vencido { get; set; }
        #endregion
        public DateTime fechaFactura { get; set; }
        public DateTime fechaVence { get; set; }
        public bool esPropuesta { get; set; }
    }
}
