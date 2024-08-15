using Core.Enum.Contabilidad;
using Core.Enum.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceCuenta
    {
        public int id { get; set; }
        public int conceptoId { get; set; }
        public TipoOperacionEnum tipoOperacion { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cc { get; set; }
        public int? acArea { get; set; }
        public int? acCuenta { get; set; }
        public string areaCuenta { get; set; }
        public bool invertirSigno { get; set; }
        public bool esCuentaDolar { get; set; }
        public TipoCuentaEnum tipoCuentaId { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("conceptoId")]
        public virtual tblEF_BalanceConcepto concepto { get; set; }

        [ForeignKey("tipoCuentaId")]
        public virtual tblEF_TipoCuenta tipoCuenta { get; set; }
    }
}
