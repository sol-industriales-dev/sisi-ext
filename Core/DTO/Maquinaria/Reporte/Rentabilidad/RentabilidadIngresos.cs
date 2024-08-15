using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class RentabilidadIngresos
    {
        public string noEconomico { get; set; }
        public string modelo { get; set; }
        public decimal rentaEquipo { get; set; }
        public decimal fletesEquipo { get; set; }
        public decimal danioEquipo { get; set; }
        public decimal ventaActivoFijo { get; set; }
        public decimal repNeumatico { get; set; }
        public decimal reservaOverhaul { get; set; }
        public decimal total { get; set; }
        public List<RentabilidadDTO> lst { get; set; }
    }
}
