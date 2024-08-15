using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class usuariosEncuestasDTO
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public string Cliente { get; set; }
        public string Empresa { get; set; }
        public string centroCostos { get; set; }
        public string accion { get; set; }
        public string CC { get; set; }
        public string departamento { get; set; }
        public string cveEmpleado { get; set; }
        public bool crearEncuesta { get; set; }
    }
}
