using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblS_Req_Condicionante
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
