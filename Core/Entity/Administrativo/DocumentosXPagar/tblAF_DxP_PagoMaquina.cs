using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_PagoMaquina
    {
        public int Id { get; set; }
        public int PagoId { get; set; }
        public int ContratoMaquinaId { get; set; }
        public bool Estatus { get; set; }
        public decimal montoPagado { get; set; }
        public DateTime fechaPago { get; set; }

//        [ForeignKey("PagoId")]
   //     public virtual tblAF_DxP_Pago Pago { get; set; }
       // [ForeignKey("ContratoMaquinaId")]
     //   public virtual tblAF_DxP_ContratoMaquina ContratoMaquina { get; set; }
    }
}