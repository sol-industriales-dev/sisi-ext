using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Plantillas
{
    [Table("SITUACION")]
    public class SITUACION
    {
        [Key]
        public string CODIGO { get; set; }
        public string DESCRIPCION { get; set; }
        public string CODIGO_RTPS { get; set; }
    }
}