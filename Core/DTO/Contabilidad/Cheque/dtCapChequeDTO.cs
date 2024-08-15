using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Cheque
{
    public class dtCapChequeDTO
    {
        public string pageseA { get; set; }
        public string fecha { get; set; }
        public int cheque { get; set; }
        public string cuenta { get; set; }
        public string cantidad { get; set; }
        public string cantidadLetra { get; set; }
    }
}
