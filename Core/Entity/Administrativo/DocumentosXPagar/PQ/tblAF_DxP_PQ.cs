using Core.DTO;
using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar.PQ
{
    public class tblAF_DxP_PQ : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public int folio { get; set; }
        public int bancoId { get; set; }
        public DateTime fechaFirma { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string cc { get; set; }
        public int monedaId { get; set; }
        public decimal importe { get; set; }
        public decimal interes { get; set; }
        public decimal? tipoCambio { get; set; }
        public int tipoMovimientoId { get; set; }
        public int ctaAbonoBanco { get; set; }
        public int sctaAbonoBanco { get; set; }
        public int ssctaAbonoBanco { get; set; }
        public int digitoAbonoBanco { get; set; }
        public int ctaCargoBanco { get; set; }
        public int sctaCargoBanco { get; set; }
        public int ssctaCargoBanco { get; set; }
        public int digitoCargoBanco { get; set; }
        public int archivoId { get; set; }
        public DateTime? fechaLiquidacion { get; set; }
        public string poliza { get; set; }

        [ForeignKey("bancoId")]
        public virtual tblAF_DxP_Institucion banco { get; set; }

        [ForeignKey("monedaId")]
        public virtual tblC_TipoMoneda moneda { get; set; }

        [ForeignKey("tipoMovimientoId")]
        public virtual tblAF_DxP_PQ_TipoMovimiento tipoMovimiento { get; set; }

        [ForeignKey("archivoId")]
        public virtual tblAF_DxP_PQ_Archivo archivoPQ { get; set; }

        [ForeignKey("pqID")]
        public virtual List<tblAF_DxP_PQ_Abono> abonos { get; set; }
    }
}
