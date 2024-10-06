using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_CC_Autorizaciones
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int empleado { get; set; }
        public int usuario { get; set; }
        public int num_autorizaciones { get; set; }
        public decimal monto_minimo { get; set; }
        public decimal monto_maximo { get; set; }
        public int tipo_auth { get; set; }
        public bool activo_fijo { get; set; }
        public int orden { get; set; }
    }
}
