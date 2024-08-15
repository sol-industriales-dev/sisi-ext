using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class CuentaDTO
    {
        public string descripcion { get; set; }
        public string tipo { get; set; }
        public int cuenta { get; set; }
        public int moneda { get; set; }
        public int banco { get; set; }
    }
}
