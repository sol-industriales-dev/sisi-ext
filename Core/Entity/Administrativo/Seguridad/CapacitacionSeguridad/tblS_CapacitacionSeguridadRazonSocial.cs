using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadRazonSocial
    {
        public int id { get; set; }
        public string razonSocial { get; set; }
        public string rfc { get; set; }
        public bool estatus { get; set; }

        public virtual List<tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial> relacionCCDepartamentoRazonSocial { get; set; }
        public int division { get; set; }
    }
}
