using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionRelacionCCAutorizante
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int usuarioID { get; set; }
        public PuestoAutorizanteEnum tipoPuesto { get; set; }
        public int orden { get; set; }
        public bool estatus { get; set; }
        public int division { get; set; }
    }
}
