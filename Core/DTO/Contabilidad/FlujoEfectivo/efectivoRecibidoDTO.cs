using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class efectivoRecibidoDTO
    {
        public string numcte { get; set; }
        public string factura { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public decimal total { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string fechafac { get; set; }
    }
}
