using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.ControlObra.Gestion
{
    public class firmasDTO
    {
        public int? id { get; set; }
        public int idOrdenDeCambio { get; set; }
        public string nombreEmpleado { get; set; }
        public string Cliente { get; set; }
        public string rutaFirma {get; set; }
        public int? idUsuarioFirma { get; set; }
        public HttpPostedFileBase ArchivoFirmado { get; set; }
        public string puestoFirmante { get; set; }
    }
}
