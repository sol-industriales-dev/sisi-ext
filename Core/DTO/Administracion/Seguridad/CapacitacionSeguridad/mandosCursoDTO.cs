using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class mandosCursoDTO
    {
        public int id { get; set; }
        public int curso_id { get; set; }
        public int mando { get; set; }
        public bool estatus { get; set; }
    }
}
