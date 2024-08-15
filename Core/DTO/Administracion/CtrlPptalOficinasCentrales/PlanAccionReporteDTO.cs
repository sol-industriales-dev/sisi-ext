using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class PlanAccionReporteDTO
    {
        public string planAccion { get; set; }
        public string resultado { get; set; }
        public string concepto { get; set; }
        public string descEstado { get; set; }
        public bool esRetrasado { get; set; }
    }
}
