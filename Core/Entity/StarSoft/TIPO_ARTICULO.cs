using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft
{
    public class TIPO_ARTICULO
    {
        [Key]
        public string COD_TIPO { get; set; }
        public string DES_TIPO { get; set; }
        public bool FLG_VENTA { get; set; }
    }
}
