using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionIHHControlActividad
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int cantidad { get; set; }
        public int colaboradorCapacitacionId { get; set; }
        public bool estatus { get; set; }


        [ForeignKey("colaboradorCapacitacionId")]
        public virtual tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion { get; set; }
    }
}
