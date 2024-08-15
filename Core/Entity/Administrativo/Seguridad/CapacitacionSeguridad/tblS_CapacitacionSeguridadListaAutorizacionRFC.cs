using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadListaAutorizacionRFC
    {
        public int id { get; set; }
        public string rfc { get; set; }
        public string razonSocial { get; set; }
        public int listaAutorizacionID { get; set; }
        public bool estatus { get; set; }
    }
}
