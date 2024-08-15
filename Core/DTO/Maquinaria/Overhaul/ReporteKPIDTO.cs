using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Captura;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ReporteKPIDTO
    {
        public int idMaquina { get; set; }
        public string noEconomico { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public kpiInfoGeneralDTO[] disponibilidad = new kpiInfoGeneralDTO[12];
    }
}
