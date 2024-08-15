using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_Subcuenta
    {
        public int Id { get; set; }
        public int CuentaId { get; set; }
        public int Subcuenta { get; set; }
        public int SubSubcuenta { get; set; }
        public string Descripcion { get; set; }
        public bool EsMaquinaria { get; set; }
        public bool EsOverhaul { get; set; }
        public int? meses { get; set; }
        public decimal? porcentaje { get; set; }
        public string ConceptoDepreciacion { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("CuentaId")]
        public virtual tblC_AF_Cuenta Cuenta { get; set; }

        [ForeignKey("SubCuentaId")]
        public virtual List<tblC_AF_InfoDepreciacion> InfoDepreciacion { get; set; }

        [ForeignKey("SubcuentaId")]
        public virtual List<tblC_AF_RelacionCuentaAño> RelAño { get; set; }
    }
}