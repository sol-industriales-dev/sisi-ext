using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class FormatoAutorizacionDTO
    {
        public string nombreCurso { get; set; }
        public string claveCurso { get; set; }
        public string revision { get; set; }
        public string fechaExpedicion { get; set; }
        public string fechaVencimiento { get; set; }
        public string rfc { get; set; }
        public string razonSocial { get; set; }
        public string nota { get; set; }
        public string nombreJefe { get; set; }
        public string firmaJefe { get; set; }
        public string nombreGerente { get; set; }
        public string firmaGerente { get; set; }
        public string nombreCoordinador { get; set; }
        public string firmaCoordinador { get; set; }
        public string nombreSecretario { get; set; }
        public string firmaSecretario { get; set; }
        public string referenciaNormativa { get; set; }
        public List<AsistenteCapacitacionDTO> asistentes { get; set; }
    }
}
