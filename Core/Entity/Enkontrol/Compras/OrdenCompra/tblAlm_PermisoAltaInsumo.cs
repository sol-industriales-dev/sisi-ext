using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_PermisoAltaInsumo
    {
        public int id { get; set; }
        public string familia { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
        public int usuarioID { get; set; }
        public bool estatus { get; set; }
    }
}
