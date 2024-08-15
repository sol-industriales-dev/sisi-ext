using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado
{
    public class tblC_RelCatReservaTp
    {
        public int id { get; set; }
        public int idCatReserva { get; set; }
        /// <summary>
        /// TipoProrrateoReservaEnum
        /// </summary>
        public int idTipoProrrateo { get; set; }
        public string tp { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
