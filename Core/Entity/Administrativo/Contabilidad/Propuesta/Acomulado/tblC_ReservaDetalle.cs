using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado
{
    public class tblC_ReservaDetalle
    {
        public int id { get; set; }
        public int idReserva { get; set; }
        public DateTime fecha { get; set; }
        public int tipo { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        //public decimal porcentaje { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public bool esActivo { get; set; }
        public DateTime ultimoCambio { get; set; }
    }
}
