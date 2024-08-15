using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblS_Req_CentroCostoDivision
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int division { get; set; }
        public int lineaNegocio_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public bool estatus { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
    }
}
