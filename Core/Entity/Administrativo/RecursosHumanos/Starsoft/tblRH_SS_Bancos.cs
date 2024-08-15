using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Starsoft
{
    public class tblRH_SS_Bancos
    {
        public int id { get; set; }
        public string CODBANCO { get; set; }
        public string NOMBRE { get; set; }
        public string CODIGO_RTPS { get; set; }
        public decimal INTCTS { get; set; }
        public string CODIGO_RTPS2 { get; set; }
    }
}
