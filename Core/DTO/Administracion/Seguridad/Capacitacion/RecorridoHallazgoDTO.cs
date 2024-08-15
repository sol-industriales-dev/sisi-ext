using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class RecorridoHallazgoDTO
    {
        public int id { get; set; }
        public string deteccion { get; set; }
        public string recomendacion { get; set; }
        public ClasificacionHallazgoEnum clasificacion { get; set; }
        public string clasificacionDesc { get; set; }
        public string rutaEvidencia { get; set; }
        public int evaluador { get; set; }
        public bool solventada { get; set; }
        public int recorridoID { get; set; }
        public bool estatus { get; set; }

        public List<int> listaLideres { get; set; }
        public string lideresString { get; set; }
        public bool puedeEvaluar { get; set; }
    }
}
