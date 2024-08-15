using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_BloqueoCCMovimiento
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int tipo_mov { get; set; }
        public bool estatus { get; set; }
    }
}
