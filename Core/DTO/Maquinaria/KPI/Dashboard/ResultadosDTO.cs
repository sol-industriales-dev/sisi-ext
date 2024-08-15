using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class ResultadosDTO
    {
        public TablaTiemposDTO tiempos { get; set; }
        public List<CalculosDTO> disVsUti_economico { get; set; }
        public List<CalculosDTO> disVsUti_modelo { get; set; }
        public List<CalculosDTO> disVsUti_grupo { get; set; }

        public InfoGraficasDTO gpx_disVsUti_economico { get; set; }
        public InfoGraficasDTO gpx_disVsUti_modelo { get; set; }
        public InfoGraficasDTO gpx_disVsUti_grupo { get; set; }
        public InfoGraficasDTO gpx_disVsUti_semanal { get; set; }
        public InfoGraficasDTO gpx_disVsUti_mensual { get; set; }

        public InfoGraficasDTO gpx_opeVsTra_economico { get; set; }
        public InfoGraficasDTO gpx_opeVsTra_modelo { get; set; }
        public InfoGraficasDTO gpx_opeVsTra_grupo { get; set; }
        public InfoGraficasDTO gpx_opeVsTra_semanal { get; set; }
        public InfoGraficasDTO gpx_opeVsTra_mensual { get; set; }

        public InfoGraficasDTO gpx_UT_economico { get; set; }
        public InfoGraficasDTO gpx_UT_modelo { get; set; }
        public InfoGraficasDTO gpx_UT_grupo { get; set; }
        public InfoGraficasDTO gpx_UT_semanal { get; set; }
        public InfoGraficasDTO gpx_UT_mensual { get; set; }
    }
}
