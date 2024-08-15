using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_Unidades
    {
        public int id { get; set; }
        public string unidad { get; set; }
        public bool estatus { get; set; }
    
        public virtual List<tblCO_Unidades_Actividad> unidadesCostos { get; set; }
    }
}
