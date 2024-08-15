using Core.Enum.Administracion.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Requerimientos
{
    public class RelacionEmpleadoAreaCCDTO
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int empleado { get; set; }
        public int area { get; set; }
        public string areaDesc { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int division { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombreCompleto { get; set; }
        public int puesto { get; set; }
        public string puestoDesc { get; set; }
        public bool esContratista { get; set; }
    }
}
