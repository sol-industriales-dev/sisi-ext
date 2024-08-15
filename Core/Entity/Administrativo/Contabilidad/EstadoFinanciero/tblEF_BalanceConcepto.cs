using Core.Enum.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceConcepto
    {
        public int id { get; set; }
        public int grupoId { get; set; }
        public string descripcion { get; set; }
        public int orden { get; set; }
        public bool esSubtitulo { get; set; }
        public bool tieneEnlace { get; set; }
        public TipoDetalleEnum tipoDetalleId { get; set; }
        public TipoIndicadorEnum? tipoIndicador { get; set; }
        public bool calcularDolaresClientes { get; set; }
        public bool calcularDolaresProveedores { get; set; }
        public bool esAcumulado { get; set; }
        public bool dxpLpCirculante { get; set; }
        public bool dxpLp { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("grupoId")]
        public virtual tblEF_BalanceGrupo grupo { get; set; }

        [ForeignKey("conceptoId")]
        public virtual ICollection<tblEF_BalanceCuenta> cuentas { get; set; }
    }
}
