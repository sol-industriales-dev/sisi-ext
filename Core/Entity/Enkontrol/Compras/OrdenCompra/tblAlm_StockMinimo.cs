using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_StockMinimo
    {
        public int id { get; set; }
        public int almacenID { get; set; }
        public int insumo { get; set; }
        public string stockMinimo { get; set; }
        public bool estatus { get; set; }
    }
}
