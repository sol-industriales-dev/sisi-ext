using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru
{
    public class tblC_AF_AjusteDepreciacionPeru
    {
        public int id { get; set; }
        public DateTime fechaAjuste { get; set; }
        public decimal montoAjuste { get; set; }
        public int relacionPolizaID { get; set; }
        public int tipoAjusteDepID { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("relacionPolizaID")]
        public virtual tblC_AF_RelacionPolizaPeru relacionPoliza { get; set; }

        [ForeignKey("tipoAjusteDepID")]
        public virtual tblC_AF_TipoAjusteDepreciacionPeru tipoAjuste { get; set; }
    }
}
