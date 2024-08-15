using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionIHHEquipoAdiestramiento
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public TipoEquipoAdiestramientoEnum tipo { get; set; }
        public bool registroActivo { get; set; }
    }
}
