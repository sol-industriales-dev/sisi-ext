using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class reporteDesechoDTO
    {
        public int idReporte { get; set; }
        public string noEconomico { get; set; }
        public string modelo { get; set; }
        public decimal horasMaquina { get; set; }
        public string serieMaquina { get; set; }
        public string subconjunto { get; set; }
        public string numParte { get; set; }
        public string serieComponente { get; set; }
        public decimal horasComponente { get; set; }
        public decimal horasAcumuladas { get; set; }
        public string rutaFotoSerie { get; set; }
        public string motivo { get; set; }
        public string realizo { get; set; }
        public int realizoID { get; set; }        
        public string firma { get; set; }
        public string fecha { get; set; }
        public string evidencia { get; set; }
    }
}
