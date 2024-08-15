using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CatSubCategoriasHH
    {
        public int id { get; set; }
        public int categoriaID { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
    }
}
