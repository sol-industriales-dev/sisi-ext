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
    public class tblC_AF_RelacionPolizaPeruAjuste : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public int yearPoliza { get; set; }
        public int mesPoliza { get; set; }
        public int polizaPoliza { get; set; }
        public string tpPoliza { get; set; }
        public int lineaPoliza { get; set; }
        public int ctaPoliza { get; set; }
        public int sctaPoliza { get; set; }
        public int ssctaPoliza { get; set; }
        public int tmPolizaId { get; set; }
        public string referenciaPoliza { get; set; }
        public string ccPoliza { get; set; }
        public string conceptoPoliza { get; set; }
        public decimal montoPoliza { get; set; }
        public DateTime fechaPoliza { get; set; }
        public int tipoPolizaDeAjusteId { get; set; }
        public int relacionPolizaPeruId { get; set; }

        [ForeignKey("tmPolizaId")]
        public virtual tblC_TipoMovimiento tmPoliza { get; set; }

        [ForeignKey("tipoPolizaDeAjusteId")]
        public virtual tblC_AF_TipoPolizaDeAjuste tipoPolizaAjuste { get; set; }

        [ForeignKey("relacionPolizaPeruId")]
        public virtual tblC_AF_RelacionPolizaPeru relacionPolizaPeru { get; set; }
    }
}
