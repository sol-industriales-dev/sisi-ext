using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Captura
{
    public class kpiReporteConcentraadoDTO
    {

        public string economico { get; set; }
        public decimal horasTrabajadas { get; set; }
        public decimal horasMMTO { get; set; }
        public decimal horasReserva { get; set; }
        public decimal horasDemora { get; set; }
        public decimal utilizacion { get; set; }
        public decimal disponibilidad { get; set; }
        public decimal horasTotales { get; set; }
    }
}
