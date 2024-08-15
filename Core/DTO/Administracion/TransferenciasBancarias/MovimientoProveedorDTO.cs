using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.TransferenciasBancarias
{
    public class MovimientoProveedorDTO
    {
        public int numpro { get; set; }
        public int factura { get; set; }
        public int tm { get; set; }
        public string concepto { get; set; }
        public string cc { get; set; }
        public string referenciaoc { get; set; }
        public decimal total { get; set; }
    }
}
