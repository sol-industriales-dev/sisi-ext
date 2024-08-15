using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_ContratoDetalle
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int Parcialidad { get; set; }
        public decimal AmortizacionCapital { get; set; }
        public decimal IvaSCapital { get; set; }
        public decimal Intereses { get; set; }
        public decimal IvaIntereses { get; set; }
        public decimal Importe { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Pagado { get; set; }
        public DateTime? FechaPago { get; set; }
        public bool GeneroInteresMoratorio { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaOriginal { get; set; }

        [ForeignKey("ContratoId")]
        public virtual tblAF_DxP_Contrato Contrato { get; set; }
        [ForeignKey("PeriodoId")]
        public virtual ICollection<tblAF_DxP_Pago> Pagos { get; set; }
    }
}