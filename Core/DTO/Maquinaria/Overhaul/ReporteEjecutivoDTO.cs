using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ReporteEjecutivoDTO
    {
        public List<ActividadOverhaulDTO> actividades { get; set; }
        public List<byte[]> archivos { get; set; }
        public List<ArchivoReporteEjecutivoDTO> archivosRE { get; set; }
        public PortadaReporteEjecutivoDTO portada { get; set; }
        public string noEconomico { get; set; }
        public string fechaInicio { get; set; }
    }
}
