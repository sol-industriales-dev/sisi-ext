using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_CatSubconjuntos
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int idConjunto { get; set; }
        public virtual tblBL_CatConjuntos CatConjuntos { get; set; }
        //public string conjunto { get; set; }
        public bool esActivo { get; set; }
        public string abreviacion { get; set; }
    }
}