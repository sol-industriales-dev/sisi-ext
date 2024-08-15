using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class MovPolDiarioAcumDTO
    {
        public DateTime fechapol { get; set; }
        public int poliza { get; set; }
        public string referencia { get; set; }
        public string cc { get; set; }
        public int naturaleza { get; set; }
        public decimal monto { get; set; }
        public int moneda { get; set; }
        public int itm { get; set; }
        public string descripcion { get; set; }
        public string concepto { get; set; }
    }
}
