using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    [Table("REQUISD")]
    public class REQUISD
    {
        [Key, Column(Order = 0)]
        public string NROREQUI { get; set; }
        public string CODPRO { get; set; }
        public string DESCPRO { get; set; }
        public string UNIPRO { get; set; }
        public decimal CANTID { get; set; }
        public string ESTREQUI { get; set; }
        public DateTime FECREQUE { get; set; }
        [Key, Column(Order = 1)]
        public decimal REQITEM { get; set; }
        public decimal SALDO { get; set; }
        public string CENCOST { get; set; }
        public string GLOSA { get; set; }
        public string REMAQ { get; set; }
        public string ORDFAB_REQUI { get; set; }
        [Key, Column(Order = 2)]
        public string TIPOREQUI { get; set; }
    }
}
