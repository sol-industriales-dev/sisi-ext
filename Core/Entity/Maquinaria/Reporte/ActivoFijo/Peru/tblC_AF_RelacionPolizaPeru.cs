using Core.DTO;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru
{
    public class tblC_AF_RelacionPolizaPeru : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public bool esMaquina { get; set; }
        public int? idActivo { get; set; }
        public string numEconomico { get; set; }
        public DateTime fechaMovimiento { get; set; }
        public DateTime? fechaInicioDep { get; set; }
        public decimal? porcentajeDep { get; set; }
        public int? mesesDep { get; set; }
        public int? relacionPolizaPeruId_baja { get; set; }
        public bool capturaAutomatica { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public int yearPoliza { get; set; }
        public int mesPoliza { get; set; }
        public int polizaPoliza { get; set; }
        public string tpPoliza { get; set; }
        public int lineaPoliza { get; set; }
        public int tmPolizaId { get; set; }
        public int ctaPoliza { get; set; }
        public int sctaPoliza { get; set; }
        public int ssctaPoliza { get; set; }
        public string ccPoliza { get; set; }
        public string referenciaPoliza { get; set; }
        public string conceptoPoliza { get; set; }
        public decimal montoPoliza { get; set; }
        public DateTime fechaPoliza { get; set; }
        public string factura { get; set; }
        public string concepto { get; set; }

        [ForeignKey("relacionPolizaPeruId_baja")]
        public virtual tblC_AF_RelacionPolizaPeru relacionPolizaPeru_baja { get; set; }

        [ForeignKey("tmPolizaId")]
        public virtual tblC_TipoMovimiento tmPoliza { get; set; }

        [ForeignKey("relacionPolizaPeruId")]
        public virtual List<tblC_AF_RelacionPolizaPeruAjuste> relacionPolizaPeruAjuste { get; set; }
    }
}
