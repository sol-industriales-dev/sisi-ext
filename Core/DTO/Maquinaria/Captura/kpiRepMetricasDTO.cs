using Core.DTO.Maquinaria.Captura.KPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class kpiRepMetricasDTO
    {
        public List<kpiAnualDTO> AnualDTO { get; set; }
        public List<kpiMTGraficaTiemposParoDTO> kpiMTGraficaTiemposParo { get; set; }
        public List<kpiMTGraficaDisponibilidadDTO> kpiMTGraficaDisponibilidad { get; set; }
        public List<kpiMTGraficaTendenciaMTTODTO> kpiMTGraficaTendenciaMTTO { get; set; }
        public List<kpiMTGraficaTiposMTTODTO> kpiMTGraficaTiposMTTO { get; set; }
    }
}
