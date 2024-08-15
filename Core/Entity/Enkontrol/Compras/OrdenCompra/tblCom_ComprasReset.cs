using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Enkontrol.Compras;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_ComprasReset
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public TipoComprasResetEnum tipo { get; set; }
        public bool registroAplica { get; set; }
        public bool estatus { get; set; }
    }
}
