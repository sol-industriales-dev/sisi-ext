using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    [Table("CUENTA_TELE_ANEXO")]
    public class CUENTA_TELE_ANEXO
    {
        [Key, Column(Order = 0)]
        public string ANEXO { get; set; }
        [Key, Column(Order = 1)]
        public string BAN_CODIGO { get; set; }
        [Key, Column(Order = 2)]
        public string CTABAN_CODIGO { get; set; }
        public string MON_CODIGO { get; set; }
        public int? TIPO_CTA { get; set; }
        public string BAN_CODIGO_INTERBAN { get; set; }
        public int? TIPO_CTA_INTERBAN { get; set; }
        public int? TIPO_CTA_CBK { get; set; }
    }
}
