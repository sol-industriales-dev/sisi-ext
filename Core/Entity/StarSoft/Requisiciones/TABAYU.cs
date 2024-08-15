using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    [Table("TABAYU")]
    public class TABAYU
    {
        [Key, Column(Order = 0)]
        public string TCOD { get; set; }
        [Key, Column(Order = 1)]
        public string TCLAVE { get; set; }
        public string TDESCRI { get; set; }
        public string TDIAVEN { get; set; }
        public string TNIVEL { get; set; }
        public DateTime? TDATE { get; set; }
        public string THORA { get; set; }
        public bool TRESTA { get; set; }
        public bool TADVALOR { get; set; }
        public bool TNOFOB { get; set; }
    }
}
