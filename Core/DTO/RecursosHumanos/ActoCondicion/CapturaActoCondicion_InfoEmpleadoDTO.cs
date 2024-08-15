using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion
{
    public class CapturaActoCondicion_InfoEmpleadoDTO
    {
        public int clave_empleado { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public string nombreCompleto { get; set; }
        public DateTime? fecha_alta { get; set; }
        public string puestoDescripcion { get; set; }
        public string empresa { get; set; }
    }
}
