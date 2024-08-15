using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_Fisico
    {
        public int id { get; set; }
        public string centro_costo { get; set; }
        public int almacen { get; set; }
        public DateTime fecha { get; set; }
        public decimal total { get; set; }
        public string estatus { get; set; }
        public decimal total_entrada { get; set; }
        public decimal total_salida { get; set; }
        public int empleado { get; set; }
        public int origen { get; set; }
        public bool registroActivo { get; set; }
    }
}
