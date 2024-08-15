using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_EncCaratula_Concideracion
    {
        public int id { get; set; }
        public int EncCaratula { get; set; }
        public int ConsideracionCostoHora { get; set; }
        public bool isActivo { get; set; }
    }
}
