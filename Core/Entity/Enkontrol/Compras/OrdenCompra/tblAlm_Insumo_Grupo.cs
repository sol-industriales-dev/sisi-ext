using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_Insumo_Grupo
    {
        public int id { get; set; }
        public int grupo { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
    }
}
