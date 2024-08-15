using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Plantillas
{
    [Table("TIPOSTRAB")]
    public class TIPOSTRAB
    {
        [Key]
        public string TIPTRAB { get; set; }
        public string DESCRIP { get; set; }
        public string CODIGO_RTPS { get; set; }
    }
}