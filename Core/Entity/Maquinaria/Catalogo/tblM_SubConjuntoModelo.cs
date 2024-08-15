using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_SubConjuntoModelo
    {

        public int id { get; set; }
        public int modeloID { get; set; }
        public int subConjuntoID { get; set; }
        public virtual tblM_CatModeloEquipo modelo { get; set; }
        public virtual tblM_CatSubConjunto SubConjunto { get; set; }


    }
}
