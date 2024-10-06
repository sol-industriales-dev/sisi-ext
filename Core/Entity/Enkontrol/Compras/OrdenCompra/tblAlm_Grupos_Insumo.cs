using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_Grupos_Insumo
    {
        public int id { get; set; }
        public int familia { get; set; }
        public int tipo_insumo { get; set; }
        public int grupo_insumo { get; set; }
        public string descripcion { get; set; }
        public string inventariado { get; set; }
        public string valida_area_cta { get; set; }
    }
}
