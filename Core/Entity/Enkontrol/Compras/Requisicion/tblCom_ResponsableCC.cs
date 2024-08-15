using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.Requisicion
{
    public class tblCom_ResponsableCC
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int empleado { get; set; }
        public bool registroActivo { get; set; }
    }
}
