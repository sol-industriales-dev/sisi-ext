using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.TransferenciasBancarias
{
    public class ProveedorDTO
    {
        public int numpro { get; set; }
        public string nombre { get; set; }
        public string rfc { get; set; }
        public string beneficiario { get; set; }
        public int? banco { get; set; }
        public string bancoDesc { get; set; }
        public int? moneda { get; set; }
        public string cuenta { get; set; }
        public string sucursal { get; set; }
        public int? plaza { get; set; }
        public string clabe { get; set; }
        public string email { get; set; }
    }
}
