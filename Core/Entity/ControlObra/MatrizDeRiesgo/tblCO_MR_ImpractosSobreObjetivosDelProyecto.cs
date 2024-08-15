using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.MatrizDeRiesgo
{
    public class tblCO_MR_ImpractosSobreObjetivosDelProyecto
    {
        public int id { get; set; }
        public int idMatriz { get; set; }
        public int tipo { get; set; }
        public string tiempo { get; set; }
        public string costo { get; set; }
        public string calidad { get; set; }
        public int baja { get; set; }
        public int bajaFin { get; set; }
    }
}
