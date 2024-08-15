using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class SaldoGlobalDTO
    {
        /// <summary>
        /// TipoSaldoGlobalEnum
        /// </summary>
        public string clase { get; set; }
        public int orden { get; set; }
        public string descripcion { get; set; }
        public decimal saldo { get; set; }
        public decimal total { get; set; }
        public decimal global { get; set; }
    }
}
