using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class IngresosXCobrarCxCDTO
    {
        public int idConceptoDir { get; set; }
        public int numcte { get; set; }
        public int factura { get; set; }
        public string cc { get; set; }
        public decimal monto { get; set; }
        public DateTime fechaFactura { get; set; }
        public string descripcion { get; set; }
        public int tipo { get; set; }
        public bool esActivo { get; set; }
    }
}
