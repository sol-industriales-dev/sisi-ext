using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class KPIHomologadoDetDTO
    {
        public string codigo { get; set; }
        public string economico { get; set; }
        public string ac { get; set; }
        public decimal valor { get; set; }
        public string fecha { get; set; }
        public string turno { get; set; }
    }
}
