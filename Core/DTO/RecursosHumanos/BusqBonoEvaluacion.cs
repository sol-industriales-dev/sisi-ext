using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class BusqBonoEvaluacion
    {
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public List<string> lstPuestos { get; set; }
    }
}
