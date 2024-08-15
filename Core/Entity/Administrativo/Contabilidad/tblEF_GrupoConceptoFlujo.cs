using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_GrupoConceptoFlujo
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("grupoReporteID")]
        public virtual ICollection<tblEF_EdoFinancieroConcepto> conceptos { get; set; }
    }
}
