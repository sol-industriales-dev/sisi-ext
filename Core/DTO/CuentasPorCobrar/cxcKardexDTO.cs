using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.CuentasPorCobrar
{
    public class cxcKardexDTO
    {
        public int numcte { get; set; }
        public int factura { get; set; }
        public string cc { get; set; }
        public decimal total { get; set; }
        public string nombreCliente { get; set; }
        public string ccDesc { get; set; }

    }
}
