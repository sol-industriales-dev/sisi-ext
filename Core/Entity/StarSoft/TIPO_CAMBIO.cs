using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft
{
    [Table("TIPO_CAMBIO")]
    public class TIPO_CAMBIO
    {
        public string TIPOMON_CODIGO { get; set; }
        [Key]
        public DateTime TIPOCAMB_FECHA { get; set; }
        public decimal TIPOCAMB_COMPRA { get; set; }
        public decimal TIPOCAMB_EQCOMPRA { get; set; }
        public decimal TIPOCAMB_VENTA { get; set; }
        public decimal TIPOCAMB_EQVENTA { get; set; }
    }
}
