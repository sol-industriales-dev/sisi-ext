using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_CierreInventarioFisico
    {
        public int id { get; set; }
        public int almacen { get; set; }
        public DateTime fecha { get; set; }
        public int usuarioCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool estatus { get; set; }
    }
}
