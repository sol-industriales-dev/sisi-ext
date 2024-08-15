using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class ActividadAvanceDTO
    {
        public int id { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public DateTime fechaI { get; set; }
        public DateTime fechaF { get; set; }
        public int? periodoAvance { get; set; }


        public bool autorizado { get; set; }
        public bool estatus { get; set; }

        public int capitulo_id { get; set; }
        public string capitulo { get; set; }
    }
}
