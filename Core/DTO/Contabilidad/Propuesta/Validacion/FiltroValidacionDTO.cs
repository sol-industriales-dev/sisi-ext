using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta.Validacion
{
    public class FiltroValidacionDTO
    {
        public int id { get; set; }
        public bool esAuth { get; set; }
        public int tm { get; set; }
        public int factura { get; set; }
        public decimal total { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
    }
}
