using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_CatJornadasPersonalCC
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int diasSemana { get; set; }
        public int hrsTrabajadasDias { get; set; }
    }
}
