using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    public class AREA
    {
        [Key]
        public string AREA_CODIGO { get; set; }
        public string AREA_DESCRIPCION { get; set; }
    }
}
