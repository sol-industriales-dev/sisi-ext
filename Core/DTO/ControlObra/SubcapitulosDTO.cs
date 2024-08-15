using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class SubcapitulosDTO
    {
        public int capitulo_id { get; set; }
        public int? subcapituloN3_id { get; set; }
        public int? subcapituloN2_id { get; set; }
        public int? subcapituloN1_id { get; set; }

        public string capitulo { get; set; }
        public string subcapituloN1 { get; set; }
        public string subcapituloN2 { get; set; }
        public string subcapituloN3 { get; set; }
    }
}
