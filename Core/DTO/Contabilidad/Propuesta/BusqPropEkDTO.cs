using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class BusqPropEkDTO
    {
        public DateTime fechaCorte { get; set; }
        //public DateTime fechaPago { get; set; }
        public int tipoProceso { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public List<string> lstCc { get; set; }
        public bool tipo { get; set; }
    }
}
