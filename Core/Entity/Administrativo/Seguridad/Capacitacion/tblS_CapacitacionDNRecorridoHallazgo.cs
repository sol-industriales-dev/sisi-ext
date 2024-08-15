using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.Capacitacion;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNRecorridoHallazgo
    {
        public int id { get; set; }
        public string deteccion { get; set; }
        public string recomendacion { get; set; }
        public ClasificacionHallazgoEnum clasificacion { get; set; }
        public string rutaEvidencia { get; set; }
        public int evaluador { get; set; }
        public bool solventada { get; set; }
        public int recorridoID { get; set; }
        public bool estatus { get; set; }
    }
}
