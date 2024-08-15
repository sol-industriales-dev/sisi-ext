using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNCicloTrabajoControlAreaSeguimiento
    {
        public int id { get; set; }
        public int areaSeguimientoId { get; set; }
        public int cicloTrabajoId { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("areaSeguimientoId")]
        public virtual tblS_CapacitacionDNCicloTrabajoAreaSeguimiento areaSeguimiento { get; set; }

        [ForeignKey("cicloTrabajoId")]
        public virtual tblS_CapacitacionDNCicloTrabajoRegistro cicloTrabajo { get; set; }
    }
}
