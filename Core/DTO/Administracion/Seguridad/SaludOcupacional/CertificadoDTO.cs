using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.SaludOcupacional
{
    public class CertificadoDTO
    {
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string fecha { get; set; }
        public int edad { get; set; }
        public string TA { get; set; }
        public string FC { get; set; }
        public string FR { get; set; }
        public string SPO { get; set; }
        public string temperatura { get; set; }
        public string talla { get; set; }
        public string peso { get; set; }
        public string impresionDiagnostica { get; set; }
        public string medicoNombre { get; set; }
        public string medicoCedula { get; set; }
    }
}
