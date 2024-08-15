using Core.DTO.Contabilidad.ControlPresupuestal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Reportes
{
    public class tendenciaGeneralDTO
    {
        public List<tendenciasDTO> datosEmpresa { get; set; }
        public List<tendenciasDTO> datos { get; set; }
        public List<tendenciasDTO> datosOtros { get; set; }
        public List<tendenciasDTO> datosConcentrado { get; set; }
        public GraficaDTO grafica_tendencia { get; set; }
        public GraficaDTO grafica_barras { get; set; }
        public bool esMultiCuenta { get; set; }
        public bool tieneOtros { get; set; }
    }
}
