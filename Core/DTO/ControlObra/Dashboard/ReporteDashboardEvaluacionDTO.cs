using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.Dashboard
{
    public class ReporteDashboardEvaluacionDTO
    {
        public string cc { get; set; }
        public string contrato { get; set; }
        public string nombreSucC { get; set; }
        public string periodoEval { get; set; }
        public decimal calificacionGlobal { get; set; }
        public int realizadas { get; set; }
        public int aprobadas { get; set; }
        public int noAprobadas { get; set; }
        public decimal porcAprobadas { get; set; }
        public List<GraficaCumplimientoElementosDTO> lstElementosEvaluados { get; set; }
        public decimal cumplimientoTiempo { get; set; }
        public decimal cumplimientoCompromiso { get; set; }
    }
}
