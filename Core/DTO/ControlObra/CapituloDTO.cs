using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class CapituloDTO
    {
        public int id { get; set; }
        public string capitulo { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public int? cc_id { get; set; }
        public string cc { get; set; }
        public int? autorizante_id { get; set; }
        public int? periodoFacturacion { get; set; }
        public string periodoFact { get; set; }
    }
}
