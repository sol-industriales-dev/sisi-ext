using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceTipoBalance
    {
        public int id { get; set; }
        public int tipo { get; set; }
        public string descripcion { get; set; }
        public bool esConsolidado { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("tipoBalanceId")]
        public virtual ICollection<tblEF_BalanceGrupo> grupos { get; set; }
    }
}
