using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_Comprador_Admin
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int empleado { get; set; }
        public int sn_empleado { get; set; }
        public bool estatus { get; set; }
    }
}
