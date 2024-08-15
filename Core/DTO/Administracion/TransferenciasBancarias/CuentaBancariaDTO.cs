using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.TransferenciasBancarias
{
    public class CuentaBancariaDTO
    {
        public int cuenta { get; set; }
        public string descripcion { get; set; }
        public int banco { get; set; }
        public int moneda { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string tp { get; set; }
        public int ultimo_cheque { get; set; }
        public long? num_cta_banco { get; set; }
        public string clabe { get; set; }
    }
}
