using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Cheque
{
    public class gastos_provDTO
    {
        public int numpro { get; set; }
        public int referenciaoc { get; set; }
        public string cc { get; set; }
        public string tm { get; set; }
        public int factura { get; set; }
        public decimal monto { get; set; }
        public decimal total { get; set; }
        public int cfd_folio { get; set; }
        public decimal tipocambio { get; set; }
        public decimal iva { get; set; }     
    }
}
