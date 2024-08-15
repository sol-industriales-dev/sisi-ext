using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Catalogos
{
    public class tblP_CatTipoSangre
    {
        public int id { get; set; }
        public string tipoSangre { get; set; }
        public bool esActivo { get; set; }
    }
}