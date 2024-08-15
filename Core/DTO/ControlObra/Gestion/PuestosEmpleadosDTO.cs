using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.Gestion
{
    public class PuestosEmpleadosDTO
    {
        public string clave_empleado { get; set; }
        public string nombre_completo { get; set; }
        public string puesto { get; set; }
        public string descripcion { get; set; }
    }
}
