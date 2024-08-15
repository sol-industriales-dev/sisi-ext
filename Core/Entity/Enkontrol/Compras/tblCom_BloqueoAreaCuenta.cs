using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_BloqueoAreaCuenta
    {
        public int id { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public bool registroActivo { get; set; }
    }
}
