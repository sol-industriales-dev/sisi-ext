using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class repLiberacionDTO
    {

        public string nombreCompleto { get; set; }
        public string nombrePuesto { get; set; }
        public string ccDescripcion { get; set; }
        public string fechaAlta { get; set; }
        public string fechaBaja { get; set; }
        public string motivo { get; set; }
        public string contratable { get; set; }
        public int? idEmpleado { get; set; }
        public int? idJefe { get; set; }
        public string cc { get; set; }
    }
}
