using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar
{
    public class AmortizacioContratoDTO
    {
        public int Parcialidad { get; set; }
        public decimal AmortizacionCapital { get; set; }
        public decimal IVASCapital { get; set; }
        public decimal Interes { get; set; }
        public decimal IVAInteres { get; set; }
        public decimal Importe { get; set; }
        public decimal Saldo { get; set; }
        public string FechaVencimiento { get; set; }
        public string Intereses { get; set; }
        public int Id { get; set; }
    }
}
