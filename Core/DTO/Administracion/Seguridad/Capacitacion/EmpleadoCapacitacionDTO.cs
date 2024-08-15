using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class EmpleadoCapacitacionDTO
    {
        public int claveEmpleado { get; set; }
        public string curp { get; set; }
        public string nombreEmpleado { get; set; }
        public string puestoEmpleado { get; set; }
        public int puestoID { get; set; }
        public string departamentoEmpleado { get; set; }
        public int departamentoID { get; set; }
        public DateTime fechaAlta { get; set; }
        public string ccID { get; set; }
        public string cc { get; set; }
    }
}