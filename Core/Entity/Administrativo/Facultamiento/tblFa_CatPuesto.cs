using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Facultamiento
{
    public class tblFa_CatPuesto
    {
        public int id { get; set; }
        public int idFacultamiento { get; set; }
        public int idTabla { get; set; }
        public int orden { get; set; }
        public string puesto { get; set; }
    }
}
