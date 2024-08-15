using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class ReporteProgramadoDTO
    {
        public string realizo { get; set; }
        public string enterado { get; set; }
        public string fechaFooter { get; set; }
        public string horometroActual { get; set; }
        public List<ReporteMiscelaneosDTO> miscelaneos { get; set; }
        public List<string> leyendas { get; set; }
        public List<ReporteActExtDNsDTO> actExtDNs { get; set; }
        public List<tblM_BitacoraActividadesMantProy> ActividadesMantPro { get; set; }

        public string componentes1 { get; set; }
        public string componentes2 { get; set; }
        public string componentes3 { get; set; }
        public string componentes4 { get; set; }
        public string componentes5 { get; set; }
        public string comentarios{ get; set; }
    }
}
