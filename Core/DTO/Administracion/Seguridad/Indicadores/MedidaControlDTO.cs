using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class MedidaControlDTO
    {
        public string accionPreventiva { get; set; }
        public string fechaCump { get; set; }
        public string responsableNombre { get; set; }
        public string estatus { get; set; }
        public string prioridad { get; set; }
        public string informeID { get; set; }
    }
}
