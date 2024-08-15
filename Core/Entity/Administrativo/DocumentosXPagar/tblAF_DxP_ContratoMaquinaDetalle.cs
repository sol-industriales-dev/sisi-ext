using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_ContratoMaquinaDetalle
    {
        public int Id { get; set; }
        public int ContratoMaquinaId { get; set; }
        public int ContratoDetalleId { get; set; }
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

        [ForeignKey("ContratoMaquinaId")]
        public virtual tblAF_DxP_ContratoMaquina ContratoMaquina { get; set; }
        [ForeignKey("ContratoDetalleId")]
        public virtual tblAF_DxP_ContratoDetalle ContratoDetalle { get; set; }
    }
}