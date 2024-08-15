using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionRelacionCCDepartamentoRazonSocial
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string departamento { get; set; }
        public int razonSocialID { get; set; }
        public int empresa { get; set; }
        public bool estatus { get; set; }
        public int division { get; set; }

        public virtual tblS_CapacitacionRazonSocial razonSocial { get; set; }
    }
}
