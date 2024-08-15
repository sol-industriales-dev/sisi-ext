using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class AspectosAmbientalesToTrayectosDTO
    {
        public int id { get; set; }
        public int idAgrupacion { get; set; }
        public List<int> lstAspectosAmbientalesID { get; set; }
        public string tratamiento { get; set; }
        public string manifiesto { get; set; }
        public DateTime fechaEmbarque { get; set; }
        public string tipoTransporte { get; set; }
        public int idTransportistaTrayecto { get; set; }
        public string codigoContenedor { get; set; }
        public int idAspectoAmbiental { get; set; }
        public string aspectoAmbiental { get; set; }
        public int idArchivoTrayecto { get; set; }
        public int clasificacion { get; set; }
        public int idCaptura { get; set; }
    }
}
