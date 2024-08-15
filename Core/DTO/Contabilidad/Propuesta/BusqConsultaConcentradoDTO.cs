using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class BusqConsultaConcentradoDTO
    {
        public DateTime min { get; set; }
        public DateTime max { get; set; }
        public List<string> lstCC { get; set; }
        public List<string> lstTp { get; set; }
        public List<string> lstCta { get; set; }
        public List<string> lstTm { get; set; }
        public bool esDivIndustrial { get; set; }
    }
}
