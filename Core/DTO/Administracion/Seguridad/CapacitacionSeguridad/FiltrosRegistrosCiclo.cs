using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class FiltrosRegistrosCiclo
    {
        public List<string> listaCC { get; set; }
        public List<int> listaCiclos { get; set; }
        public List<RegistrosCicloAreasDTO> listaAreas { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}
