using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.ActoCondicion
{
    public class tblRH_AC_ActoAccionReaccion
    {
        public int id { get; set; }
        public int actoID { get; set; }
        public int accionReaccionID { get; set; }
        public bool seleccionado { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("actoID")]
        public virtual tblRH_AC_Acto acto { get; set; }

        [ForeignKey("accionReaccionID")]
        public virtual tblRH_AC_AccionReaccionContactoPersonal accionReaccion { get; set; }
    }
}
