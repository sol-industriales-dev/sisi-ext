using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.Dashboard
{
    public class ReporteDashboardEvaluacionesContratoDTO
    {
        public int id_evaluacion { get; set; }
        public int id_contrato { get; set; }
        public int contratoAutorizadas { get; set; }
        public int contratoRequeridas { get; set; }
        public int contratoCompromisoAutorizadas { get; set; }
        public int contratoCompromismoRequeridas { get; set; }
        public int contratoTiempoAutorizadas { get; set; }
        public int contratoTiempoRequeridas { get; set; }
    }
}
