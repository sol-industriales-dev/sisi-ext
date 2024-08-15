using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class AccidentabilidadTopDTO
    {
        public string titulo { get; set; }
        public List<AccidentabilidadDTO> lst { get; set; }
    }
    public class AccidentabilidadDTO
    {
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public decimal porcentaje { get; set; }
    }
}
