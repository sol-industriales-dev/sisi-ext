using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionListaAutorizacionCC
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int departamento { get; set; }
        public int empresa { get; set; }
        public int listaAutorizacionID { get; set; }
        public bool estatus { get; set; }
    }
}
