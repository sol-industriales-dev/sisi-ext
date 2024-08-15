using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.ActoCondicion
{
    public class tblRH_AC_AccionReaccionContactoPersonal
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int tipo { get; set; }
        public bool estatus { get; set; }
    }
}
