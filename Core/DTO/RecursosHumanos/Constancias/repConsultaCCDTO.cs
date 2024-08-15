using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class repConsultaCCDTO
    {
        public string clave_empleado { get; set; }
        public string nombreCompleto { get; set; }
        public string nombrePuesto { get; set; }
        public string contratable { get; set; }
        public int idUsu { get; set; }
        public string usuario { get; set; }
    }
}
