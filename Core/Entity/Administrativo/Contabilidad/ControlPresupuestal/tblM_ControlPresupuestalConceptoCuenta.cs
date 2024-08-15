using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.ControlPresupuestal
{
    public class tblM_ControlPresupuestalConceptoCuenta
    {
        public int id { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int conceptoID { get; set; }
        public bool estatus { get; set; }
    }
}
