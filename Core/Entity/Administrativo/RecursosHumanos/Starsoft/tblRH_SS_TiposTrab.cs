using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Starsoft
{
    public class tblRH_SS_TiposTrab
    {
        public int id { get; set; }
        public int TIPTRAB { get; set; }
        public string DESCRIP { get; set; }
        public int? CODIGO_RTPS { get; set; }

    }
}
