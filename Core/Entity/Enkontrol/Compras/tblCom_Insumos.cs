using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_Insumos
    {
        public int id { get; set; }
        public int insumo { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public bool bit_area_cuenta { get; set; }
        public string cancelado { get; set; }
        public int color_resguardo { get; set; }
        public int compras_req { get; set; }
    }
}
