using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_CatModeloEquipotblM_CatSubConjunto
    {
        public int id { get; set; }
        public int modeloID { get; set; }
        public int subconjuntoID { get; set; }
        public string numParte { get; set; }
        //public virtual tblM_CatModeloEquipo modelo { get; set; }
        public virtual tblM_CatSubConjunto subconjunto { get; set; }
    }
}

