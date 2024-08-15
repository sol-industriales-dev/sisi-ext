using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_InsumosConsignaPeru
    {
        public int id { get; set; }
        public int insumo { get; set; }
        public string proveedor { get; set; }
        public decimal precio { get; set; }
        public bool registroActivo { get; set; }
    }
}
