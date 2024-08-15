using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoCompromisos
{
    public class ActividadDashboardDTO
    {
        public int actividad_id { get; set; }
        public string actividadDesc { get; set; }
        public int diasRestantes { get; set; }
        public decimal avance { get; set; }
        public int responsable_id { get; set; }
        public string responsableDesc { get; set; }
        public decimal porcentaje { get; set; }
    }
}
