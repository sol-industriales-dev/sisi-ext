using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_AutorizanteCentroCosto
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string areaCuenta { get; set; }
        public int empleado { get; set; }
        public bool registroActivo { get; set; }
    }
}
