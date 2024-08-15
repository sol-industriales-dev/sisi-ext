using Core.DTO.Administracion.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad
{
    public class ReporteGlobalDTO
    {
        public string tituloPeriodo { get; set; }
        public string rangoPeriodo { get; set; }
        public List<incidentesRegistrablesXmes> tasaIncidentesAnual { get; set; }
        public List<incidentesRegistrablesXmes> incidentesPorMesTRIFR { get; set; }
        public List<incidenciasPresentadasDTO> incidenciasPresentadas { get; set; }
        public List<incidenciasPresentadasDTO> incidenciasPresentadasGlobal { get; set; }
    }
}
