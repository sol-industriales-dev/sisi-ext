using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Plantilla
{
    public class PlantillaReporteDTO
    {
        public string cc { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public List<PlantillaPersonalDTO> data { get; set; }
        public List<PlantillaAutorizanteDTO> autorizantes { get; set; }
    }
}
