using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesTipoProcedimientosViolados
    {
        public int id { get; set; }
        public string Procedimineto { get; set; }
        public virtual List<tblS_IncidentesInformePreliminar> lstInformes { get; set; }
    }
}
