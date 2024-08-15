using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class EmpleadoDTO
    {
        public int clave_empleado { get; set; }
        public string puesto { get; set; }
        public string tipo_nomina { get; set; }
        public string cc_contable { get; set; }
    }
}
