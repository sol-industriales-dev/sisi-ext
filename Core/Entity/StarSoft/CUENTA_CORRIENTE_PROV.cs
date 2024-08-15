using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.EntityFramework.Mapping.StarSoft
{


    [Table("CUENTA_CORRIENTE_PROV")]
    public class CUENTA_CORRIENTE_PROV
    {
        [Key]
        public string ANEXO { get; set; }
        public string BAN_CODIGO { get; set; }
        public string CTABAN_CODIGO { get; set; }
        public string MON_CODIGO { get; set; }

    }
}
