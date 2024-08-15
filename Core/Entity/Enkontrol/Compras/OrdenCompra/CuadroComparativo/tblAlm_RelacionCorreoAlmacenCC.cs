using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    public class tblAlm_RelacionCorreoAlmacenCC
    {
        public int id { get; set; }
        public int almacen { get; set; }
        public int almacenistaID { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string areaCuenta { get; set; }
        public int compradorID { get; set; }
        public bool estatus { get; set; }
    }
}
