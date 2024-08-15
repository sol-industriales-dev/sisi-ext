using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class centrosCostoCursoDTO
    {
        public int id { get; set; }
        public int curso_id { get; set; }
        public string cc { get; set; }
        public bool estatus { get; set; }
    }
}
