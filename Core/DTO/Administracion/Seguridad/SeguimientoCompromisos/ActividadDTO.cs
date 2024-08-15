using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.SeguimientoCompromisos;
using Core.Entity.SeguimientoCompromisos;

namespace Core.DTO.SeguimientoCompromisos
{
    public class ActividadDTO
    {
        public int id { get; set; }
        public int agrupacion_id { get; set; }
        public string agrupacionDesc { get; set; }
        public int area { get; set; }
        public string areaDesc { get; set; }
        public int areaEvaluadora { get; set; }
        public string areaEvaluadoraDesc { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public ClasificacionActividadSCEnum clasificacion { get; set; }
        public string clasificacionDesc { get; set; }
        public decimal porcentaje { get; set; }
        public decimal progresoReal { get; set; }
        public List<tblSC_Evidencia> evidencias { get; set; }
    }
}
