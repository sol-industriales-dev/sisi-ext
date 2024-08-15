using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contratistas
{
    public class tblS_IncidentesEmpresasContratistas
    {
        public int id { get; set; }
        public string nombreEmpresa { get; set; }
        public bool esActivo { get; set; }
    }
}
