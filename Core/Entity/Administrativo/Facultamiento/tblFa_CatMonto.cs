using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Facultamiento
{
    public class tblFa_CatMonto
    {
        public int id { get; set; }
        public int idFacultamiento { get; set; }
        public int idTabla { get; set; }
        public int renglon { get; set; }
        public decimal max { get; set; }
        public decimal min { get; set; }
    }
}
