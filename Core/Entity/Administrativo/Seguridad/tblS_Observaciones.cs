using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad
{
    public class tblS_Observaciones
    {
        public int id { get; set; }
        public int idVehiculo { get; set; }
        public int idTipo { get; set; }
        public int idParte { get; set; }
        public int anterior { get; set; }
        public int actual { get; set; }
        public string observaciones { get; set; }
    }
}
