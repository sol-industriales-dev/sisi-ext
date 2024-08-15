using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Cheque
{
    public class filtroCheques
    {
        public int cuenta { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public bool permiso { get; set; }
    }
}
