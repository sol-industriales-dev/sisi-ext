using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesTipoLesion
    {
        public int id { get; set; }
        public string tipoLesion { get; set; }

        public virtual List<tblS_Incidentes> Incidentes { get; set; }
        public virtual List<tblS_IncidentesInformePreliminar> Informes { get; set; }
    }
}
