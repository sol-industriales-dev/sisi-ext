using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_RelacionAlmacenPrincipalVirtual
    {
        public int id { get; set; }
        public int almacenPrincipal { get; set; }
        public int almacenVirtual { get; set; }
        public bool estatus { get; set; }
    }
}
