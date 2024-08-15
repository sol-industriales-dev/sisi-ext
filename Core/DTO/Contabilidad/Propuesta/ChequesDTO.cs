using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class ChequesDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public DateTime fecha_mov { get; set; }
        public decimal monto { get; set; }
        public decimal tc { get; set; }
        public string descripcion { get; set; }
        public string concepto { get; set; }
        public int cuenta { get; set; }
        public int tm { get; set; }
        public int scta { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
    }
}
