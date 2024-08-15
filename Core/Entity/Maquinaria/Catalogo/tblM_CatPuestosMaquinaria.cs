using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_CatPuestosMaquinaria
    {
        public int id { get; set; }
        public int puesto { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
    }
}
