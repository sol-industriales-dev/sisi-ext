using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class ReporteInteresesPagadosDTO
    {
        public string nombre { get; set; }
        public string Moneda { get; set; }
        public int Cta { get; set; }
        public int ctaIA { get; set; }
        public int Scta { get; set; }
        public int Sscta { get; set; }
        public string Contrato { get; set; }
        public string Folio { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal Pagado { get; set; }
        public decimal CP { get; set; }
        public decimal LP { get; set; }
        public decimal sumaCPLP { get; set; }
    }
}
