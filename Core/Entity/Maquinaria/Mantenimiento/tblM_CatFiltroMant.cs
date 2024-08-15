using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_CatFiltroMant
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int  marca { get; set; }
        public string modelo { get; set; }
        public bool  estado { get; set; }
        public bool sintetico { get; set; }
    }
}
