using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class SubcapitulosNivel2DTO
    {
        public int id { get; set; }
        public string subcapitulo { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public int subcapituloN1_id { get; set; }
        public string subcapituloN1 { get; set; }
        public int capitulo_id { get; set; }
        public string capitulo { get; set; }
    }
}
