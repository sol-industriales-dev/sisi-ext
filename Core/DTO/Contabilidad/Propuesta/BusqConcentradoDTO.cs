using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class BusqConcentradoDTO
    {
        public DateTime min { get; set; }
        public DateTime max { get; set; }
        public List<string> lstCC { get; set; }
        public List<string> ctrlCC { get; set; }
        public bool esDivIndustrial { get; set; }
    }
}
