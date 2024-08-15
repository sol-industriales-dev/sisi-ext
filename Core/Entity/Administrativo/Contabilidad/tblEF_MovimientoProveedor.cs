using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_MovimientoProveedor
    {
        public int id { get; set; }
        public int numeroProveedor { get; set; }
        public int tipoMovimiento { get; set; }
        public string cc { get; set; }
        public int? area { get; set; }
        public int? cuenta { get; set; }
        public string areaCuenta { get; set; }
        public decimal total { get; set; }
        public int corteMesID { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("corteMesID")]
        public virtual tblEF_CorteMes corte { get; set; }
    }
}
