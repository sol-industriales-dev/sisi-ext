using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class empleadoIncidenteDTO
    {
        public decimal claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public DateTime edadEmpleado { get; set; }
        public string puestoEmpleado { get; set; }
        public DateTime antiguedadEmpleado { get; set; }
        public string antiguedadEmpleadoStr { get; set; }
        public string turnoEmpleado { get; set; }
        public string supervisorEmpleado { get; set; }
        public string ccID { get; set; }
        public string cc { get; set; }
        public string deptoEmpleado { get; set; }
        public string responsableCC { get; set; }
        public int FK_responsableCC { get; set; }
    }
}
