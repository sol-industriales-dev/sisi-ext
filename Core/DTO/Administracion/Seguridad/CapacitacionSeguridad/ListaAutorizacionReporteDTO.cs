using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class ListaAutorizacionReporteDTO
    {
        public string claveListaAutorizacion { get; set; }
        public string revision { get; set; }
        public string codigoCurso { get; set; }
        public string fechaEmision { get; set; }
        public string razonSocial { get; set; }
        public string rfc { get; set; }
        public string departamento { get; set; }
        public string objetivoContenidoCapacitacion { get; set; }
        public string nota { get; set; }
        public string nombreJefe { get; set; }
        public string firmaJefe { get; set; }
        public string nombreGerente { get; set; }
        public string firmaGerente { get; set; }
        public string nombreCoordinador { get; set; }
        public string firmaCoordinador { get; set; }
        public string nombreSecretario { get; set; }
        public string firmaSecretario { get; set; }
        public string nombreSeguridad { get; set; }
        public string firmaSeguridad { get; set; }
        public string referenciaNormativa { get; set; }

        public List<ListaAsistentesReporteDTO> listaAsistentes { get; set; }
    }
}
