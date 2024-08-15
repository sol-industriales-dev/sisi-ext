using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblS_Req_Color
    {
        public int id { get; set; }
        public string rgb { get; set; }
        public string hex { get; set; }
        public bool estatus { get; set; }
    }
}
