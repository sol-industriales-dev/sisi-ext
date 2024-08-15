using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.Capacitacion;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNCicloTrabajoCriterio
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public PonderacionCriterioEnum ponderacion { get; set; }
        public AspectoEvaluadoCriterioEnum aspectoEvaluado { get; set; }
        public int cicloTrabajoID { get; set; }
        public bool estatus { get; set; }
    }
}
