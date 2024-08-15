using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class FiltrosCambiosDTO
    {

        public List<string> cc { get; set; }
        public List<string> concepto { get; set; }
        public List<string> empleado { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}
