using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.SaludOcupacional
{
    public class AtencionMedicaReporteDTO
    {
        public string nombreEmpleado { get; set; }
        public string fechaIngreso { get; set; }
        public string puesto { get; set; }
        public string edad { get; set; }
        public string supervisor { get; set; }
        public string fechaAtencionMedica { get; set; }
        public string area { get; set; }
        public string tipo { get; set; }

        public List<RevisionReporteDTO> revisiones { get; set; }
    }
}
