using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class planeacionPrincipalDTO
    {
        public int conceptoID { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public int tipoConcepto { get; set; }
    }
}
