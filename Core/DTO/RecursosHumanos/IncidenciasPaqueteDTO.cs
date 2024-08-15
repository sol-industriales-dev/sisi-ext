using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class IncidenciasPaqueteDTO
    {
        public List<IncidenciasEmpDetDTO> datos { get; set; }
        public List<IncidenciasObservacionesDTO> observaciones { get; set; }
        public List<IncidenciasObservacionesDTO> observacionesBonno { get; set; }
        public BusqIncidenciaDTO busq { get; set; }
        public tblRH_BN_Incidencia incidencia { get; set; }
        public List<tblRH_BN_Incidencia_det> incidencia_det { get; set; }
        public List<tblRH_BN_Incidencia_det_Peru> incidencia_det_Peru { get; set; }
        public bool isAuth { get; set; }
        public bool permiso_bono_sinlimite { get; set; }
        public bool evaluacion_pendiente { get; set; }
        public bool isDesauth { get; set; }
    }
}
