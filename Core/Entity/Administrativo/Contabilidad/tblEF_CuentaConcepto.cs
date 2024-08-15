using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_CuentaConcepto
    {
        public int id { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cc { get; set; }
        public int conceptoID { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("conceptoID")]
        public virtual tblEF_EdoFinancieroConcepto concepto { get; set; }
    }
}
