using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Contabilidad.ControlPresupuestal;

namespace Core.Entity.Administrativo.Contabilidad.ControlPresupuestal
{
    public class tblM_ControlPresupuestalConcepto
    {
        public int id { get; set; }
        public ConceptoEnum concepto { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
    }
}
