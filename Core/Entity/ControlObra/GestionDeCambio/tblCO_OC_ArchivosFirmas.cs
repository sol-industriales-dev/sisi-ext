using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.GestionDeCambio
{
    public class tblCO_OC_ArchivosFirmas
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string ruta { get; set; }
        public bool esActivo { get; set; }
    }
}
