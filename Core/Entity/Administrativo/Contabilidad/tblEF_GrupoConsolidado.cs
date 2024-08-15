using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_GrupoConsolidado
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("grupoId")]
        public virtual ICollection<tblEF_ConceptoConsolidados> conceptos { get; set; }
    }
}
