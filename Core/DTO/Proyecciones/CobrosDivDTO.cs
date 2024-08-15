using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Proyecciones
{
    public class CobrosDivDTO
    {
        public MesDTO ln1CliPorcentajeSaldoAmortizar { get; set; }
        public MesDTO ln2ImporteAmortizar1 { get; set; }
        public MesDTO ln3ImporteAmortizar2 { get; set; }
        public MesDTO ln4CxCPorcentajeSaldoAmortizar { get; set; }
        public MesDTO ln5CxCImporteAmortizar { get; set; }
        public MesDTO ln6AporteCapital { get; set; }
    }
}
