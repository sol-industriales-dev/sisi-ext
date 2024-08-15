using Core.DTO.Maquinaria.KPI.Autorizaciones;
using Core.DTO.Maquinaria.KPI.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI
{
    public class ResultadosReporteDTO
    {
        public List<CalculosDTO> disVsUti_modeloDiario { get; set; }
        public List<CalculosDTO> disVsUti_modeloSemanal { get; set; }
        public List<CalculosDTO> disVsUti_modeloMensual { get; set; }
        public List<DatosGraficasDTO> gpx_disVsUti_modeloDiario { get; set; }
        public List<DatosGraficasDTO> gpx_disVsUti_modeloSemanal { get; set; }
        public List<DatosGraficasDTO> gpx_disVsUti_modeloMensual { get; set; }

    }
}
