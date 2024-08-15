using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_EconomicoPuedeAnsul
    {
        public int id { get; set; }
        public int modelEquipoId { get; set; }
        public string descripcionModeloEquipo { get; set; }
        public bool registroActivo { get; set; }

    }
}
