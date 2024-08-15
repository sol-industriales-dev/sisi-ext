using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class AvancesAutorizarDTO
    {
        public int avance_id { get; set; }
        public int capitulo_id { get; set; }
        public int? cc_id { get; set; }
        public string capitulo { get; set; }
        public string cc { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string nombreAutorizante { get; set; }

    }
}
