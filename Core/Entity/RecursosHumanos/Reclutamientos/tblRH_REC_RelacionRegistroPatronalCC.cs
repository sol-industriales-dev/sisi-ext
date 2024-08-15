using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_RelacionRegistroPatronalCC
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int clave_reg_pat { get; set; }
        public bool registroActivo { get; set; }
    }
}
