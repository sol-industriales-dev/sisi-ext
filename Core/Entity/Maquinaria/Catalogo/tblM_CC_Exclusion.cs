using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_CC_Exclusion
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int baseDatos { get; set; }
        public bool estatus { get; set; }
    }
}
