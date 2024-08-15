using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class RepCambiosDTO
    {
        public string cC { get; set; }
        public string empleadoID { get; set; }
        public string empleado { get; set; }
        public string fechaCambioStr { get; set; }
        public DateTime fechaCambio { get; set; }
        public string cambios { get; set; }
        public int cPuesto { get; set; }
        public int cSueldo { get; set; }
        public int cJeIn { get; set; }
        public int cCC { get; set; }
        public int cRePa { get; set; }
        public int cTiN { get; set; }
    }
}
