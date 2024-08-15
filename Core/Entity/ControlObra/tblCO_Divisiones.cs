using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_Divisiones
    {
        public int id { get; set; }
        public string division { get; set; }
        public bool estatus { get; set; }

        public virtual List<tblCO_PlantillaInforme> plantillasInformes  { get; set; }
        public virtual List<tblCO_DivisionCC> centrosCostos { get; set; }
    }
}
