using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_CatMarcaEquipotblM_CatGrupoMaquinaria
    {
        public int id { get; set; }
        public int tblM_CatMarcaEquipo_id { get; set; }
        public int tblM_CatGrupoMaquinaria_id { get; set; }
        public bool? isActivo { get; set; }
    }
}
