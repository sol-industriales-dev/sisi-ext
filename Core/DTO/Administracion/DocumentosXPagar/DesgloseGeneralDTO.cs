using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class DesgloseGeneralDTO
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int Parcialidad { get; set; }
        public decimal AmortizacionCapital { get; set; }
        public decimal IVASCapital { get; set; }
        public decimal Interes { get; set; }
        public decimal IVAInteres { get; set; }
        public decimal Importe { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Pagado { get; set; }
        public DateTime? FechaPago { get; set; }
        public bool Intereses { get; set; }
    }
}