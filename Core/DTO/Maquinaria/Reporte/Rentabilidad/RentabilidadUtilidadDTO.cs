using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class RentabilidadUtilidadDTO
    {
        public string noEconomico { get; set; }
        public string modelo { get; set; }
        //public decimal costoHorario { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public decimal total { get; set; }
        public List<RentabilidadDTO> detalles { get; set; }
    }
}
