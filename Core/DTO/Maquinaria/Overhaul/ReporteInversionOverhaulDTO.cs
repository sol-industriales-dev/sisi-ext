using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ReporteInversionOverhaulDTO
    {
        public int paroID { get; set; }
        public int numMes { get; set; }
        public string mes { get; set; }
        public string equipo { get; set; }
        public decimal ritmo { get; set; }
        public string componente { get; set; }
        public decimal horasComponente { get; set; }
        public decimal target { get; set; }
        public string proximoPCR { get; set; }
        public decimal presupuesto { get; set; }
        public decimal erogado { get; set; }
        public int numTipoParo { get; set; }
        public string tipoParo { get; set; }
        public string subconjunto { get; set; }
        public string fechaRemocion { get; set; }
        public bool paroTerminado { get; set; }
        public bool programado { get; set; }
    }
}
