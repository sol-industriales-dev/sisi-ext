using Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.Colombia
{
    public class tblC_AF_CuentaColombia
    {
        public int id { get; set; }
        public int cuenta { get; set; }
        public string descripcion { get; set; }
        public int tipoCuentaId { get; set; }
        public bool colombiaMexico { get; set; }
        public int? cuentaIdMexico { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("tipoCuentaId")]
        public virtual tblC_AF_TipoCuenta tipoCuenta { get; set; }

        [ForeignKey("cuentaIdMexico")]
        public virtual tblC_AF_CuentaColombia cuentaMexico { get; set; }

        [ForeignKey("cuentaId")]
        public virtual ICollection<tblC_AF_SubcuentaColombia> subcuentas { get; set; }
    }
}
