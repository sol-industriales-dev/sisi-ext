using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class IncidenteDTO
    {
        public tblS_Incidentes incidente { get; set; }
        public List<tblS_IncidentesGrupoInvestigacion> grupoTrabajo { get; set; }
        public List<tblS_IncidentesOrdenCronologico> ordenCronologico { get; set; }
        public List<tblS_IncidentesEventoDetonador> eventoDetonador { get; set; }
        public List<tblS_IncidentesCausasInmediatas> causasInmediatas { get; set; }
        public List<tblS_IncidentesCausasBasicas> causasBasicas { get; set; }
        public List<tblS_IncidentesCausasRaiz> causasRaiz { get; set; }
        public List<tblS_IncidentesMedidasControl> medidasControl { get; set; }
        public List<HttpPostedFileBase> evidencias { get; set; }
        public List<int> tipoEvidenciaRIA { get; set; }
    }
}
