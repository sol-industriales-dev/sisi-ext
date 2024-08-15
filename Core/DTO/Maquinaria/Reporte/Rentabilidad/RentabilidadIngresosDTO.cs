using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class RentabilidadIngresosDTO
    {
        public string noEconomico { get; set; }
        public string modelo { get; set; }
        public decimal rentaEquipos { get; set; }
        public decimal cobroFletes { get; set; }
        public decimal cobroDanioEquipos { get; set; }
        //public decimal ingresoVenta { get; set; }
        public decimal reparacionNeumaticos { get; set; }
        public decimal reservaOverhaul { get; set; }
        public decimal total { get; set; }
        public decimal mttoEquipos { get; set; }
        public decimal lentoMovimiento { get; set; }
        public List<RentabilidadDTO> detalles { get; set; }
    }
}
