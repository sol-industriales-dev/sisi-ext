using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class GrupoInvestigacionDTO
    {
        public string nombreEmpleado { get; set; }
        public string puestoEmpleado { get; set; }
        public string departamentoEmpleado { get; set; }
        public string informeID { get; set; }
        public bool esExterno { get; set; }
    }
}
