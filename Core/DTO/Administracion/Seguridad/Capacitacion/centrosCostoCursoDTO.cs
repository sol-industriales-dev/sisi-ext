using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class centrosCostoCursoDTO
    {
        public int id { get; set; }
        public int curso_id { get; set; }
        public string cc { get; set; }
        public int empresa { get; set; }
        public bool estatus { get; set; }
    }
}
