using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.MatrizDeRiesgo
{
    public class tblCO_MatrizDeRiesgo
    {
        public int id { get; set; }
        public int idPadre { get; set; }
        public DateTime fechaElaboracion { get; set; }
        public string cc { get; set; }
        public string nombreDelProyecto { get; set; }
        public string personalElaboro { get; set; }
        public string faseDelProyecto { get; set; }
        public bool estatus { get; set; }
    }
}
