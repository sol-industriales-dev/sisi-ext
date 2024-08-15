using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_Almacenistas
    {
        public int id { get; set; }
        public int id_usuario_sigoplan { get; set; }
        public string id_usuario_starsoft { get; set; }
        public string id_usuario_inventarios { get; set; }
        public bool esActivo { get; set; }
    }
}
