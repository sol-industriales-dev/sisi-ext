using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class rptCalendarioEjecutadoDTO
    {
        public string equipo { get; set; }
        public decimal ritmo { get; set; }
        public string componente { get; set; }
        public decimal hrsComponente { get; set; }
        public decimal target { get; set; }
        public DateTime proximoPCR { get; set; }
        public string resumen { get; set; }
        public DateTime fechaOverhaul { get; set; }
    }
}
