using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceConceptoEmpresaConsolidado
    {
        public int id { get; set; }
        public int conceptoId { get; set; }
        public int conceptoConstruplanId { get; set; }
        public int conceptoArrendadoraId { get; set; }
        public int conceptoEiciId { get; set; }
        public int conceptoIntegradoraId { get; set; }
        public int tipoOperacion { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("conceptoId")]
        public virtual tblEF_BalanceConceptoConsolidado concepto { get; set; }
    }
}
