using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tbl_TraspasosPendientesTEMP
    {
        public int id { get; set; }
        public int numeroSalida { get; set; }
        public int ordenTraspaso { get; set; }
        public int almacenOrigen { get; set; }
        public int almacenDestino { get; set; }
        public DateTime fecha { get; set; }
        public string centroCostos { get; set; }
    }
}
