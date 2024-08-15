using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Contabilidad;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_EdoFinancieroConcepto
    {
        public int id { get; set; }
        public string concepto { get; set; }
        public TipoOperacionEnum tipoOperacion { get; set; }
        public int ordenReporte { get; set; }
        public int grupoReporteID { get; set; }
        public bool estatus { get; set; }
        public bool tieneEnlace { get; set; }
        public int tipoEnlaceId { get; set; }
        public bool calculoEbitda { get; set; }
        public bool calculoIndividualEbitda { get; set; }
        public bool invertirSigno { get; set; }

        [ForeignKey("grupoReporteID")]
        public virtual tblEF_GrupoConceptoFlujo grupo { get; set; }

        [ForeignKey("conceptoID")]
        public virtual ICollection<tblEF_CuentaConcepto> cuentas { get; set; }
    }
}
