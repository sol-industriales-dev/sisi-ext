using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadDNRecorridoHallazgo
    {
        public int id { get; set; }
        public string deteccion { get; set; }
        public string recomendacion { get; set; }
        public int clasificacion { get; set; }
        public string rutaEvidencia { get; set; }
        public int evaluador { get; set; }
        public bool solventada { get; set; }
        public int recorridoID { get; set; }
        public bool estatus { get; set; }
    }
}
