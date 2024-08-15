using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado
{
    public class tblC_RelCatReservaCalculo
    {
        public int id { get; set; }
        public int idCatReserva { get; set; }
        /// <summary>
        /// TipoProrrateoReservaEnum
        /// </summary>
        public int idTipoProrrateo { get; set; }
        public int idTipoCalculo { get; set; }
        public decimal porcentaje { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
