using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_PolizaDetalle
    {
        public int Id { get; set; }
        public int PolizaId { get; set; }
        public int Linea { get; set; }
        public int RelacionCuentaAñoId { get; set; }
        public int TipoMovimientoId { get; set; }
        public string Referencia { get; set; }
        public string CC { get; set; }
        public int? CatMaquinaId { get; set; }
        public string NumeroEconomico { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public int IClave { get; set; }
        public int ITM { get; set; }
        public int? CcId { get; set; }
        public int? Area { get; set; }
        public int? Cuenta_OC { get; set; }
        public string AreaCuenta { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("PolizaId")]
        public virtual tblC_AF_Poliza Poliza { get; set; }
        [ForeignKey("RelacionCuentaAñoId")]
        public virtual tblC_AF_RelacionCuentaAño RelacionCuentaAño { get; set; }
        [ForeignKey("TipoMovimientoId")]
        public virtual tblC_AF_TipoMovimiento TipoMovimiento { get; set; }
        [ForeignKey("CatMaquinaId")]
        public virtual tblM_CatMaquina CatMaquina { get; set; }
        [ForeignKey("CcId")]
        public virtual tblP_CC CentroCosto { get; set; }
    }
}