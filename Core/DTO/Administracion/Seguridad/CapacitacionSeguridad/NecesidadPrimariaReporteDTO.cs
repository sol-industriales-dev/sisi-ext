using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class NecesidadPrimariaReporteDTO
    {
        public string proyectoObra { get; set; }
        public string area { get; set; }
        public string fecha { get; set; }
        public List<NecesidadDetectadaDTO> listaNecesidadesDetectadas { get; set; }
    }
}
