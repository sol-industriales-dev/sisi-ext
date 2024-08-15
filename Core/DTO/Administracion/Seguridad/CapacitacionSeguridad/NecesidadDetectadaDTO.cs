using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class NecesidadDetectadaDTO
    {
        public string metodo { get; set; }
        public string detecciones { get; set; }
        public int accionesCursoID { get; set; }
        public string acciones { get; set; }
        public string observaciones { get; set; }
    }
}
