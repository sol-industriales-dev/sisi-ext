using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class OrdenCompraPagoDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public int partida { get; set; }
        public int dias_pago { get; set; }
        public DateTime fecha_pago { get; set; }
        public string comentarios { get; set; }
        public string estatus { get; set; }
        public decimal porcentaje { get; set; }
        public decimal importe { get; set; }
        public decimal sub_total { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public decimal total_fac { get; set; }
        public decimal total_pag { get; set; }
    }
}
