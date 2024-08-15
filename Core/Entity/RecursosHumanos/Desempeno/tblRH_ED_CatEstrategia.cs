using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Desempeno
{
    public class tblRH_ED_CatEstrategia
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public virtual ICollection<tblRH_ED_DetMetas> lstMeta { get; set; }
    }
}
