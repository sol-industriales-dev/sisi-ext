using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNCicloTrabajoInteresados : InfoRegistroDTO
    {
        public int id { get; set; }
        public int cicloTrabajoId { get; set; }
        public int interesadoId { get; set; }
    }
}
