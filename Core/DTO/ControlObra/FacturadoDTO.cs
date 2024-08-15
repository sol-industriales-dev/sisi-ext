using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class FacturadoDTO
    {
        public int id { get; set; }
        public DateTime fecha{ get; set; }

        public bool autorizado { get; set; }
        public bool estatus { get; set; }

        public int capitulo_id { get; set; }
        public string capitulo { get; set; }
    }
}
