using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.GestionDeCambio
{
    public class tblCO_OC_Notificantes
    {
        public int id { get; set; }
        public int idOrdenDeCambio { get; set; }
        public string cvEmpleados { get; set; }
        public int idUsuario { get; set; }

    }
}
