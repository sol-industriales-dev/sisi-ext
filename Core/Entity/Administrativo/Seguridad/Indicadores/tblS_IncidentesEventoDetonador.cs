using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesEventoDetonador
    {
        public int id { get; set; }
        public string eventoDetonador { get; set; }

        public int incidente_id { get; set; }
        public virtual tblS_Incidentes Incidente { get; set; }
    }
}
