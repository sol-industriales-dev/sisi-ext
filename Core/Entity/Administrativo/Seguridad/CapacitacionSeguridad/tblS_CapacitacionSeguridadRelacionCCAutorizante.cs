using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadRelacionCCAutorizante
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int usuarioID { get; set; }
        public int tipoPuesto { get; set; }
        public int orden { get; set; }
        public bool estatus { get; set; }
        public int division { get; set; }
    }
}
