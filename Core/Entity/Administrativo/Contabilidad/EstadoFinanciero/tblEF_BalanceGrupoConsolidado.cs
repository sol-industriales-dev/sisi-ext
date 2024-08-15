using Core.Enum.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceGrupoConsolidado
    {
        public int id { get; set; }
        public int tipoBalanceId { get; set; }
        public string descripcion { get; set; }
        public int orden { get; set; }
        public bool sumarTotal { get; set; }
        public bool esGranTotal { get; set; }
        public TipoIndicadorEnum? tipoIndicador { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("tipoBalanceId")]
        public virtual tblEF_BalanceTipoBalance tipoBalance { get; set; }

        [ForeignKey("grupoId")]
        public virtual ICollection<tblEF_BalanceConceptoConsolidado> conceptos { get; set; }
    }
}
