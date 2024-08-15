using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.KPI
{
    public class kpiMTGraficaTiemposParoDTO
    {
        public string CONCEPTO { get; set; }
        public decimal DISPONIBILIDAD { get; set; }
        public decimal MTBS { get; set; }
        public decimal MTTR { get; set; }
        public decimal RATIO_MTTO { get; set; }

    }
}
