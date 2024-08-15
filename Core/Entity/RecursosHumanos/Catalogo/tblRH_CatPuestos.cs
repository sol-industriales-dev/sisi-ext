using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Catalogo
{
    public class tblRH_CatPuestos
    {
        public int puesto { get; set; }
        public string descripcion { get; set; }
        public int tipo_nomina { get; set; }
        public string cc { get; set; }
    }
}
