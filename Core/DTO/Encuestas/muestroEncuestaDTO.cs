using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class muestroEncuestaDTO
    {

        public string encuesta { get; set; }
        public List<string> porcetajeData { get; set; }
        public List<string> mesColumna { get; set; }

    }
}
