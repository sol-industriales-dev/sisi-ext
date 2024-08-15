using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesGrupoInvestigacion
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string puestoEmpleado { get; set; }
        public string departamentoEmpleado { get; set; }
        public int usuarioID { get; set; }
        public bool esExterno { get; set; }

        public int incidente_id { get; set; }
        public virtual tblS_Incidentes Incidente { get; set; }
    }
}
