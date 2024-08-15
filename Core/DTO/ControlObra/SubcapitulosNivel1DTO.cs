using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class SubcapitulosNivel1DTO
    {
        public int id { get; set; }
        public string subcapitulo { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public int capituloID { get; set; }
        public string capitulo { get; set; }
    }
}
