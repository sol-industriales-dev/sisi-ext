using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class sbCuentaDTO
    {
        public int cuenta { get; set; }
        public string descripcion { get; set; }
        public int banco { get; set; }
        public int moneda { get; set; }
        public string num_cta_banco { get; set; }
        public string descBanco { get; set; }
        public string descDivision { get; set; }
        public bool esActivo { get; set; }
    }
}
