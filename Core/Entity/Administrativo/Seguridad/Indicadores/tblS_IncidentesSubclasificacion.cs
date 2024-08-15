using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesSubclasificacion
    {
        public int id { get; set; }
        public string subclasificacion { get; set; }
        public int tipoAccidenteID { get; set; }
        public virtual tblS_IncidentesTipos tipoAccidente { get; set; }
        public bool activo { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
