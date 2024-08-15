using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_REGLA_CC
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int tipo { get; set; }
        public decimal cuota { get; set; }
    }
}
