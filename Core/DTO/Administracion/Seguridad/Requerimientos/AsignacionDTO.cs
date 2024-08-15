using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Requerimientos
{
    public class AsignacionDTO
    {
        public List<int> clasificaciones { get; set; }
        public List<MultiSegDTO> arrGrupos { get; set; }
        public DateTime fechaInicioEvaluacion { get; set; }
        public List<int> requerimientos { get; set; }
        public List<int> actividades { get; set; }
        public List<int> condicionantes { get; set; }
        public List<int> secciones { get; set; }
    }
}
