using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_ContratoPeriodo
    {
        public int id { get; set; }
        public int contratoID { get; set; }
        public int periodoID { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool actualizacion { get; set; }
        public bool validado { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("contratoID")]
        public virtual tblX_Contrato contrato { get; set; }

        [ForeignKey("periodoID")]
        public virtual tblX_PeriodoCaptura periodo { get; set; }

        [ForeignKey("contratoPeriodoID")]
        public virtual List<tblX_CapturaMensual> capturaMensual { get; set; }
    }
}
