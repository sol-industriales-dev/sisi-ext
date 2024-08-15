using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Almacen
{
    [Table("STKART")]
    public class STKART
    {
        [Key, Column(Order = 0)]
        public string STALMA { get; set; }
        [Key, Column(Order = 1)]
        public string STCODIGO { get; set; }
        public decimal? STSKDIS { get; set; }
        public decimal? STSKREF { get; set; }
        public decimal? STSKMIN { get; set; }
        public decimal? STSKMAX { get; set; }
        public decimal? STPUNREP { get; set; }
        public decimal? STSEMREP { get; set; }
        public string STTIPREP { get; set; }
        public string STUBIALM { get; set; }
        public decimal? STLOTCOM { get; set; }
        public string STTIPCOM { get; set; }
        public decimal? STSKCOM { get; set; }
        public decimal? STKPREPRO { get; set; }
        public decimal? STKPREULT { get; set; }
        public DateTime? STKFECULT { get; set; }
        public decimal? STKPREPROUS { get; set; }
        public decimal? CANTREFERENCIA { get; set; }
    }
}
