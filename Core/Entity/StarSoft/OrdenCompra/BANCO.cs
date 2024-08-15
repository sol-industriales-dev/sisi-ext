using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.OrdenCompra
{
    [Table("BANCO")]
    public class BANCO
    {
        [Key]
        public string BAN_CODIGO { get; set; }
        public string BAN_DESCRIPCION { get; set; }
        public string FMCH { get; set; }
        public string BAN_EQUIVALENTE { get; set; }
        public string BAN_VOUCHER { get; set; }
    }
}
