using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Usuario_CC
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public string cc { get; set; }
        public bool autoriza { get; set; }
        public bool permiso_bono_sinlimite { get; set; }
    }
}
