using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.OrdenCompra
{
    [Table("RESPONSABLECMP")]
    public class RESPONSABLECMP
    {
        [Key]
        public string RESPONSABLE_CODIGO { get; set; }
        public string RESPONSABLE_NOMBRE { get; set; }
    }
}
