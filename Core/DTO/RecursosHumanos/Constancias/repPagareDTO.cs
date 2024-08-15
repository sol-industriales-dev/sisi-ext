using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class repPagareDTO
    {
        public string nombreCompleto { get; set; }
        public string nombreRegPatronal { get; set; }
        public string direccionPatronal { get; set; }
        public string domicilio { get; set; }
        public string poblacion { get; set; }        
        public string telefono { get; set; }
        public string cantidadSoli { get; set; }
        public string cantidadLetra { get; set; }

        public int? idEmpleado { get; set; }
        public int? idJefe { get; set; }
        public string cc { get; set; }

    }
}
