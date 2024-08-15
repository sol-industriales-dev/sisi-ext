using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.SaludOcupacional
{
    public class RevisionDTO
    {
        public int id { get; set; }
        public int consecutivo { get; set; }
        public string diagnostico { get; set; }
        public string tratamiento { get; set; }
        public string comentarios { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public bool incapacidad { get; set; }
        public bool terminacion { get; set; }
        public int diasSiguienteRevision { get; set; }
        public int atencionMedica_id { get; set; }
    }
}
