using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_ComponenteProy
    {
        public int id { get; set; }
        public int idAct { get; set; }
        public bool estatus { get; set; }
        public bool aplicar { get; set; }
        public int tipoPm { get; set; }
    }
}
