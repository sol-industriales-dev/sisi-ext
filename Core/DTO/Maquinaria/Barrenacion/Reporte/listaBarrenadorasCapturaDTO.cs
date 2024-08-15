using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion.Reporte
{
    public class listaBarrenadorasCapturaDTO
    {
        public int id { get; set; }
        public string noEconomico { get; set; }
        public string noSerie { get; set; }
        public decimal horometro { get; set; }
        public empleadoCapturaDTO operador { get; set; }
        public empleadoCapturaDTO ayudante { get; set; }
    }
}
