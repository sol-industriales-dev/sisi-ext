using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru
{
    public class tblC_AF_RelacionCuentaYearPeru
    {
        public int id { get; set; }
        public int subcuentaId { get; set; }
        public int year { get; set; }
        public int? cuentaMovimientoId { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("subcuentaId")]
        public virtual tblC_AF_SubcuentaPeru subcuentaPeru { get; set; }

        [ForeignKey("cuentaMovimientoId")]
        public virtual tblC_AF_CuentaPeru cuentaPeru { get; set; }
    }
}
