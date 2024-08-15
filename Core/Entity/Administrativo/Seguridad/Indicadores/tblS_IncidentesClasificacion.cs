using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesClasificacion
    {
        public int id { get; set; }
        public string clasificacion { get; set; }
        public string abreviatura { get; set; }

        public int tipoEvento_id { get; set; }
        public virtual tblS_IncidentesTipoEventos tipoEvento { get; set; }

        public virtual List<tblS_IncidentesTipos> incidentesTipo { get; set; }
    }
}
