using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class DepartamentoEmpleadoDTO
    {
        public int clave_empleado { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public int clave_depto { get; set; }
        public string desc_depto { get; set; }
        public string direccion { get; set; }
        public string cc { get; set; }
        public string sindicato { get; set; }
    }
}
