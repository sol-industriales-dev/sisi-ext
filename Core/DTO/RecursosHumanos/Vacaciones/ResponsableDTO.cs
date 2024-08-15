using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class ResponsableDTO
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string nombreEmpleado { get; set; }
        public DateTime? fecha_ingreso { get; set; }
        public int clave_responsable { get; set; }
        public string nombreResponsable { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int diasIniciales { get; set; }
        public int diasTomados { get; set; }
        public int diasPendientes { get; set; }
        public bool esDiasIniciales { get; set; }
        public int diasPaternidad { get; set; }
        public int diasMatrimonio { get; set; }
    }
}
