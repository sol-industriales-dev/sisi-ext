using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    /// <summary>
    /// Reservas de Tesorería
    /// </summary>
    public class tblC_Reserva
    {
        public int id { get; set; }
        /// <summary>
        /// Tipo de reserva; tipoReservaEnum
        /// </summary>
        public int tipo { get; set; }
        public DateTime fecha { get; set; }
        public string cc { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public decimal porcentaje { get; set; }
        public bool esActivo { get; set; }
        public DateTime ultimoCambio { get; set; }
    }
}
