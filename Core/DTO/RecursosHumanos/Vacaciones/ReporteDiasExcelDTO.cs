using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class ReporteDiasExcelDTO
    {
        public int clave_empleado { get; set; }
        public string nombreCompleto { get; set; }
        public string ccDesc { get; set; }
        public string puestoDesc { get; set; }
        public DateTime fechaAlta { get; set; }
        public int añoServicio { get; set; }
        public int diasDisponibles { get; set; }
        public int diasNuevoPeriodo { get; set; }
        public int totalDiasDisp { get; set; }
        public string programacion { get; set; }
        public int diasDisfrutados { get; set; }
        public int diasRestantes { get; set; }
        public List<DateTime?> lstFechas { get; set; }
    }
}
