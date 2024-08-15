using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.Gestion
{
    public class firmaOCDTO
    {
        public int id { get; set; }
        public string nombre_completo { get; set; }
        public string puesto { get; set; }
        public string firmaDigital { get; set; }
        public bool estatus { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public bool puedeAutorizar { get; set; }
        public bool estatusAutorizacion { get; set; }
        public string claveEmpleado { get; set; }
        public int idUsuario { get; set; }
        public int estatusFirma { get; set; }
        public string comentarioRechazo { get; set; }
        public string strEstatusFirma { get; set; }

        public bool tieneArchivo { get; set; }
        public int idFirma { get; set; }
    }
}
