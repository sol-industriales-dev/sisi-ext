using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesTipos
    {
        public int id { get; set; }
        public string incidenteTipo { get; set; }

        public int clasificacion_id { get; set; }
        public virtual tblS_IncidentesClasificacion clasificacion { get; set; }

        public virtual List<tblS_Incidentes> Incidentes { get; set; }

        public virtual List<tblS_IncidentesInformePreliminar> InformesPreliminares { get; set; }
        public virtual List<tblS_IncidentesSubclasificacion> subclasificaciones { get; set; }
    }
}
