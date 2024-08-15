using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_MaquinariatblM_Componente
    {

        public int id { get; set; }
        public int tblM_CatMaquina_id { get; set; }
        public int tblM_CatComponente_id { get; set; }
        public bool estatus { get; set; }

    }
}
