using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
   public class tblS_IncidentesEmpleadosTurno
    {
       public int id { get; set; }
       public string turno { get; set; }
       public virtual List<tblS_Incidentes> Incidentes { get; set; }
       public virtual List<tblS_IncidentesInformePreliminar> Informes { get; set; }
    }
}
