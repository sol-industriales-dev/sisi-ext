using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class kpiMTTOyParoDTO
    {
        public string fecha { get; set; }
        public string horometro { get; set; }
        public string falla { get; set; }
        public decimal tiempoUtil { get; set; }
        public decimal tiempoMuerto { get; set; }
        public string programado { get; set; }
        public string tipo { get; set; }
        public decimal MTBS { get; set; }
        public decimal MTTR { get; set; }
        public int personal { get; set; }
        public decimal horasHombre { get; set; }
        public int aplicaCalculo { get; set; }
        public string tipoEnParo { get; set; }
        public decimal tiempoParo { get; set; }

    }
}
