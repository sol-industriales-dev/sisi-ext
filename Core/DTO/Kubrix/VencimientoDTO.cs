using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Kubrix
{
    public class VencimientoDTO
    {
        public string numpro { get; set; }
        public string nombre { get; set; }
        public string factura { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechavenc { get; set; }
        public int tm { get; set; }
        public string autorizapago { get; set; }
        public decimal saldo_factura { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public decimal tipocambio { get; set; }
        public string nomcorto { get; set; }
        public string corto { get; set; }
        public string telefono1 { get; set; }
        public string concepto { get; set; }
        public int moneda { get; set; }
    }
}
