using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_CorteMes
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int mes { get; set; }
        public int usuarioCapturaID { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("corteMesID")]
        public virtual ICollection<tblEF_SaldoCC> saldosCC { get; set; }
    }
}
