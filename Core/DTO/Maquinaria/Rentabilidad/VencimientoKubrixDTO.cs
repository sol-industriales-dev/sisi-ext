using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Rentabilidad
{
    public class VencimientoKubrixDTO
    {
        public int numpro { get; set; }
        public string nombre { get; set; }
        public decimal por_vencer { get; set; }
        public decimal plazo_1 { get; set; }
        public decimal plazo_2 { get; set; }
        public decimal plazo_3 { get; set; }
        public decimal plazo_4 { get; set; }
        public int factura { get; set; }
        public string moneda { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechavenc { get; set; }
        public int tm { get; set; }
        public string autorizapago { get; set; }
        public decimal saldo_factura { get; set; }
        public int plazo1 { get; set; }
        public int plazo2 { get; set; }
        public int plazo3 { get; set; }
        public int plazo4 { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public decimal tipocambio { get; set; }
        public string nomcorto { get; set; }
        public string corto { get; set; }
        public string telefono1 { get; set; }
        public decimal saldo_factura_dlls { get; set; }
        public string concepto { get; set; }
        public int moneda_id { get; set; }
    }
}
