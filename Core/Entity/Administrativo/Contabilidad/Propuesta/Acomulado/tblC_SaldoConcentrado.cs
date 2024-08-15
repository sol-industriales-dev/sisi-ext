using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado
{
    public class tblC_SaldoConcentrado
    {
        public int id { get; set; }
        /// <summary>
        /// Identificador del saldo conciliado
        /// </summary>
        public int idSaldoConciliado { get; set; }
        /// <summary>
        /// folio de origen
        /// </summary>
        public string folio { get; set; }
        public DateTime fecha { get; set; }
        public string beneficiario { get; set; }
        public string concepto { get; set; }
        public string cc { get; set; }
        public string noCheque { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public int moneda { get; set; }
        #region registro
        public bool esActivo { get; set; }
        public DateTime ultimoCambio { get; set; }
        #endregion
    }
}
