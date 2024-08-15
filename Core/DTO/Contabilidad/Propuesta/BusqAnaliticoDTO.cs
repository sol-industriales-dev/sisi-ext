using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class BusqAnaliticoDTO
    {
        public DateTime fecha { get; set; }
        public List<string> lstCC { get; set; }
        public List<string> lstTm { get; set; }
        public int provMin { get; set; }
        public int provMax { get; set; }
    }
}
