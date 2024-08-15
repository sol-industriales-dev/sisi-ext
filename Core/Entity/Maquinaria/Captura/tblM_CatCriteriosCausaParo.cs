using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CatCriteriosCausaParo
    {

        public int id { get; set; }
        public string CausaParo { get; set; }
        public string TiempoMantenimiento { get; set; }
        public string TipoParo { get; set; }
        public string DescripcionParo { get; set; }
    }
}
