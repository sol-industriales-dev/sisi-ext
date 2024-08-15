using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft
{
    [Table("TIPO_DOCU")]
    public class TIPO_DOCU
    {
        [Key]
        public string TDO_TIPDOC { get; set; }
        public string TDO_DESCRI { get; set; }
        public string TDO_CODCON { get; set; }
        public string TDO_CODSUN { get; set; }
        public bool TDO_SERIE { get; set; }
        public bool TDO_RESTA { get; set; }
        public bool TDO_SISTEMA { get; set; }
        public decimal? IMP_MINIMO_ELECTRONICA { get; set; }
        public bool? FLG_DETRACCION { get; set; }
        public decimal? IMPMIN_MN_DETRACCION { get; set; }
    }
}
