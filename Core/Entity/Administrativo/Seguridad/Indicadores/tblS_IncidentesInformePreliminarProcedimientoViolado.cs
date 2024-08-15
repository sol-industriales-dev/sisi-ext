using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesInformePreliminarProcedimientoViolado
    {
        public int id { get; set; }
        public int idInformePreliminar { get; set; }
        public int idProcedimientoViolado { get; set; }
    }
}
