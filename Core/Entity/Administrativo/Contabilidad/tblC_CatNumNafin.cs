using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_CatNumNafin
    {
        public int id { get; set; }
        public string NumProveedor { get; set; }
        public string NumNafin { get; set; }
        public int TipoMoneda { get; set; }
        public string RFC { get; set; }
        public string RazonSocial { get; set; }
        public string correo { get; set; }
        public int idTipoPropuesta { get; set; }
        public bool estatus { get; set; }
    }
}
