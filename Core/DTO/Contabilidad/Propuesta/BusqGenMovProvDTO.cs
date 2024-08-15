using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class BusqGenMovProvDTO
    {
        public int tipo { get; set; }
        public DateTime minMov { get; set; }
        public DateTime maxMov { get; set; }
        public List<string> lstCc { get; set; }
        public int minProv { get; set; }
        public int maxProv { get; set; }
    }
}
