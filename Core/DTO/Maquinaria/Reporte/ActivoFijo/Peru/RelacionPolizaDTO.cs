using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.Peru
{
    public class RelacionPolizaDTO
    {
        public int id { get; set; }
        public string tipoActivo { get; set; }
        public string nombreActivo { get; set; }
        public DateTime? fechaInicioDepDT { get; set; }
        public string fechaInicioDep { get; set; }
        public decimal porcentajeDep { get; set; }
        public int mesesDep { get; set; }
        public string cc { get; set; }
        public int polizaPoliza { get; set; }
        public string poliza { get; set; }
        public int cuenta { get; set; }
        public decimal monto { get; set; }
    }
}
