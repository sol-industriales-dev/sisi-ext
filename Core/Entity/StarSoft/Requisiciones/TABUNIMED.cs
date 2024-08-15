using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    [Table("TABUNIMED")]
    public class TABUNIMED
    {
        [Key]
        public string UM_ABREV { get; set; }
        public string UM_NOMBRE { get; set; }
        public string UM_ESTADO { get; set; }
        public string UND_FACTRON { get; set; }
    }
}
