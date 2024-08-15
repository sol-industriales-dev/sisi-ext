using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft
{
    [Table("FORMA_PAGO")]
    public class FORMA_PAGO
    {
        [Key]
        public string COD_FP { get; set; }
        public string DES_FP { get; set; }
        public int DIA_FP { get; set; }
        public string USUARIO { get; set; }
        public DateTime FEC_REG { get; set; }
        public DateTime? FEC_ACT { get; set; }
        public bool? FLG_CONT_ENTREGA { get; set; }
    }
}
