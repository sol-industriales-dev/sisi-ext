using Core.Enum.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_ConceptoConsolidados
    {
        public int id { get; set; }
        public int grupoId { get; set; }
        public string concepto { get; set; }
        public int conceptoConstruplanId { get; set; }
        public int conceptoArrendadoraId { get; set; }
        public int conceptoEiciId { get; set; }
        public int conceptoIntegradoraId { get; set; }
        public TipoOperacionEnum tipoOperacion { get; set; }
        public int ordenReporte { get; set; }
        public bool estatus { get; set; }
        public bool tieneEnlace { get; set; }
        public int tipoEnlaceId { get; set; }
        public bool invertirSigno { get; set; }

        [ForeignKey("grupoId")]
        public virtual tblEF_GrupoConsolidado grupo { get; set; }
    }
}
