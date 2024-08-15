using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class KPIReporteEquipoDTO
    {
        public List<kpiMTTOyParoDTO> kpiMTTOyParo { get; set; }
        public List<kpiMotivosParoDTO> kpiMotivosParo { get; set; }
        public kpiInfoGeneralDTO kpiInfoGeneral { get; set; }
        public kpiTipoMantenimientoDTO kpiTipoMantenimiento { get; set; }
        public kpiFrecuenciaParosDTO kpiFrecuenciaParos { get; set; }

    }
}
