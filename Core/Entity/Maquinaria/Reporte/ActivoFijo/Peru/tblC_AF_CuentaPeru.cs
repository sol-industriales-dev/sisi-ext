using Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru
{
    public class tblC_AF_CuentaPeru
    {
        public int id { get; set; }
        public int cuenta { get; set; }
        public string descripcion { get; set; }
        public int tipoCuentaId { get; set; }
        public bool peruMexico { get; set; }
        public int? cuentaIdMexico { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("tipoCuentaId")]
        public virtual tblC_AF_TipoCuenta tipoCuenta { get; set; }

        [ForeignKey("cuentaIdMexico")]
        public virtual tblC_AF_CuentaPeru cuentaMexico { get; set; }

        [ForeignKey("cuentaId")]
        public virtual ICollection<tblC_AF_SubcuentaPeru> subcuentas { get; set; }
    }
}
