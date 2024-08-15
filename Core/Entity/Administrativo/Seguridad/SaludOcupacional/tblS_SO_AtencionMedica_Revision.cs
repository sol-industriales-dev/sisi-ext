using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.SaludOcupacional
{
    public class tblS_SO_AtencionMedica_Revision
    {
        public int id { get; set; }
        public int consecutivo { get; set; }
        public string diagnostico { get; set; }
        public string tratamiento { get; set; }
        public string comentarios { get; set; }
        public DateTime fecha { get; set; }
        public bool incapacidad { get; set; }
        public bool terminacion { get; set; }
        public int diasSiguienteRevision { get; set; }
        public int atencionMedica_id { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
