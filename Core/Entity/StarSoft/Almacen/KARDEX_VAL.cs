using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Almacen
{
    [Table("KARDEX_VAL")]
    public class KARDEX_VAL
    {
        [Key, Column(Order = 2)]
        public string COD_ART { get; set; }
        public DateTime? FEC_DOC { get; set; }
        public string HOR_DOC { get; set; }
        public string COD_MOV { get; set; }
        [Key, Column(Order = 3)]
        public string TIP_TRANSA { get; set; }
        [Key, Column(Order = 1)]
        public string NUM_DOC { get; set; }
        public decimal? CAN_ART { get; set; }
        public double? PRE_UNIT { get; set; }
        public double? COS_PRO { get; set; }
        public decimal? SAL_STOCK { get; set; }
        public string COD_FAM { get; set; }
        public string SER_LOT { get; set; }
        public bool? ASIENTO { get; set; }
        public string ING_SAL { get; set; }
        [Key, Column(Order=0)]
        public string ALMACEN { get; set; }
        public int? DEITEM { get; set; }
        public double? VALOR_FINAL { get; set; }

    }
}
