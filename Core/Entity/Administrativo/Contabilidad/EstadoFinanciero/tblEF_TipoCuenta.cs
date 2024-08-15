using Core.Enum.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_TipoCuenta
    {
        public TipoCuentaEnum id { get; set; }
        public string descripcion { get; set; }

        [ForeignKey("tipoCuentaId")]
        public virtual ICollection<tblEF_BalanceCuenta> cuentas { get; set; }
    }
}
