using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class RemocionesVidaUtilDTO
    {
        public string equipo { get; set; }
        public string componente { get; set; }
        public string ordenCRC { get; set; }
        public string tipoCambio { get; set; }
        public string dlls { get; set; }
        public string mn { get; set; }
        public string costoPromedio { get; set; }
        public string fecha { get; set; }
        public string noComponente { get; set; }
        public string vida { get; set; }
        public string horometro { get; set; }
        public string horasAcumuladas { get; set; }
        public string motivo { get; set; }
        public string causa { get; set; }
    }
}
