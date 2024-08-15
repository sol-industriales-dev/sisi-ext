using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_FirmaEvaluacionDetalle
    {
        public int id { get; set; }
        public int firmaEvaluacionId { get; set; }
        public int firmanteId { get; set; }
        public string urlFirma { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreacionId { get; set; }

        [ForeignKey("firmaEvaluacionId")]
        public virtual tblX_FirmaEvaluacion firmaEvaluacion { get; set; }

        [ForeignKey("firmanteId")]
        public virtual tblX_Firmante firmante { get; set; }

        [ForeignKey("usuarioCreacionId")]
        public virtual tblP_Usuario usuarioCreacion { get; set; }
    }
}
