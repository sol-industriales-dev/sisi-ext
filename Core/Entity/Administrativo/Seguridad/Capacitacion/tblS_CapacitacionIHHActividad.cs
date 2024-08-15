using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionIHHActividad : InfoRegistroDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
