using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.StandBy
{
    public class tblM_STB_AccionActivacionEconomico
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool registroActivo { get; set; }
    }
}
