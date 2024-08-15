using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Desempeno
{
    public class MetaGraficoDTO
    {
        public string name { get; set; }
        public string stack { get; set; }
        public string type { get; set; }
        public decimal[] data { get; set; }
        public bool visible { get; set; }
        public object tooltip { get; set; }
    }
}
