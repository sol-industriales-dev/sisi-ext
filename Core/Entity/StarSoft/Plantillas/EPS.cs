using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Plantillas
{
    [Table("EPS")]
    public class EPS
    {
        [Key]
        public string CODIGO { get; set; }
        public string RUC { get; set; }
        public string DESCRIPCION { get; set; }
        public string CODIGO_RTPS { get; set; }
    }
}