using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Catalogos
{
    public class tblRH_catPuestos
    {
        public int puesto { get; set; }
        public string descripcion { get; set; }
        public string desc_corta { get; set; }
        public string descripcion_puesto { get; set; }
        public int tipo_nomina { get; set; }
        public string sindicalizado { get; set; }

    }
}
