using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class EmpleadoAutorizanteDTO
    {
        public int ConceptoID { get; set; }
        public int EmpleadoID { get; set; }
        public string NombreEmpleado { get; set; }
        public int? ClaveEmpleado { get; set; }
        public int Orden { get; set; }
        public bool Aplica { get; set; }
    }
}
