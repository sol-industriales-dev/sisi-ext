using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class examenesCursoDTO
    {
        public int curso_id { get; set; }
        public int id { get; set; }
        public int tipoExamen { get; set; }
        public string nombreExamen { get; set; }
        public string pathExamen { get; set; }
        public byte linkExamen { get; set; }
    }
}
