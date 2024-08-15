using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.Requisicion
{
    public class tblCom_ReqTipo
    {
        public int id { get; set; }
        public int tipo_req_oc { get; set; }
        public string descripcion { get; set; }
        public int dias_requisicion { get; set; }
        public bool registroActivo { get; set; }
    }
}
