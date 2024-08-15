using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadIHHControlHoras
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public decimal horas { get; set; }
        public int colaboradorCapacitacionID { get; set; }
        public bool estatus { get; set; }
    }
}
