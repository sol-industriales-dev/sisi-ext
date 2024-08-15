using Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_PolizaAltaBaja_NoMaquina
    {
        public int id { get; set; }
        public DateTime? inicioDepreciacion { get; set; }
        public decimal? porcentajeDepreciacion { get; set; }
        public int? mesesDepreciacion { get; set; }
        public int? tipoMovimientoId { get; set; }
        public int? polizaBajaId { get; set; }
        public bool capturaAutomatica { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public int yearPoliza { get; set; }
        public int mesPoliza { get; set; }
        public int polizaPoliza { get; set; }
        public string tpPoliza { get; set; }
        public int lineaPoliza { get; set; }
        public int tmPoliza { get; set; }
        public int ctaPoliza { get; set; }
        public int sctaPoliza { get; set; }
        public int ssctaPoliza { get; set; }
        public string ccPoliza { get; set; }
        public string referenciaPoliza { get; set; }
        public string conceptoPoliza { get; set; }
        public decimal montoPoliza { get; set; }
        public DateTime fechaPoliza { get; set; }
        public string factura { get; set; }
        public DateTime fechaMovimiento { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioModificacionId { get; set; }

        [ForeignKey("polizaAltaBajaId")]
        public virtual List<tblC_AF_PolizaDeAjuste> polizasAjuste { get; set; }

        [ForeignKey("tipoMovimientoId")]
        public virtual tblC_AF_CatTipoMovimiento tipoMovimiento { get; set; }

        [ForeignKey("polizaBajaId")]
        public virtual tblC_AF_PolizaAltaBaja_NoMaquina polizaBaja { get; set; }

        [ForeignKey("tmPoliza")]
        public virtual tblC_AF_TipoMovimiento tipoMovimientoPoliza { get; set; }

        [ForeignKey("usuarioCreacionId")]
        public virtual tblP_Usuario usuarioCreacion { get; set; }

        [ForeignKey("usuarioModificacionId")]
        public virtual tblP_Usuario usuarioModificacion { get; set; }
    }
}
