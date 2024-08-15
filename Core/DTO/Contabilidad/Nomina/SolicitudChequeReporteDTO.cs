using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class SolicitudChequeReporteDTO
    {
        public string cuenta { get; set; }
        public string concepto { get; set; }
        public string importe { get; set; }
        public string cuentaBanco { get; set; }
        public string totalCantidadEmpleados { get; set; }
        public string totalNomina { get; set; }
        public string fondoAhorro { get; set; }
        public string depositoOneCard { get; set; }
        public string solicita1 { get; set; }
        public string solicita2 { get; set; }
        public string autoriza { get; set; }
        public string firmaSolicita1 { get; set; }
        public string firmaSolicita2 { get; set; }
        public string firmaAutoriza { get; set; }
        public string vobo { get; set; }
        public string firmaVobo { get; set; }
        public string cuentaOCSI { get; set; }
        public string nombreEmpresa { get; set; }
        public string tipoBanco { get; set; }

        public List<SolicitudChequeReporteDetalleDTO> tabla { get; set; }
    }
}
