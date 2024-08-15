using Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_InfoDepreciacion
    {
        public int Id { get; set; }
        public int SubCuentaId { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public int MesesDepreciacion { get; set; }
        public DateTime FechaComienzo { get; set; }
        public int? TipoMovimientoId { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("SubCuentaId")]
        public virtual tblC_AF_Subcuenta SubCuenta { get; set; }

        [ForeignKey("TipoMovimientoId")]
        public virtual tblC_AF_CatTipoMovimiento TipoMovimiento { get; set; }
    }
}