using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Tablas.RH.Empleado
{
    public class sn_empleadosDTO
    {
        public int clave_empleado { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public string sexo { get; set; }
        public int tipo_nomina { get; set; }
        public int clave_depto { get; set; }
        public int puesto { get; set; }
        public string estatus_empleados { get; set; }
        public string cc_contable { get; set; }
    }
}
