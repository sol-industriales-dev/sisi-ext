using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_CatMarcaEquipo
    {

        public int id { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
        //public virtual List<tblM_CatGrupoMaquinaria> grupo { get; set; }
    }
}
