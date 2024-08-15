using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_RelacionCuentaAño
    {
        public int Id { get; set; }
        public int SubcuentaId { get; set; }
        public int Año { get; set; }
        public int? CuentaMovimientoId { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("SubcuentaId")]
        public virtual tblC_AF_Subcuenta Subcuenta { get; set; }
        [ForeignKey("CuentaMovimientoId")]
        public virtual tblC_AF_Cuenta Cuenta { get; set; }
    }
}