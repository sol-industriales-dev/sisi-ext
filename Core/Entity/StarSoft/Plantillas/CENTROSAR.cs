using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Entity.StarSoft.Plantillas
{
    [Table("CENTROSAR")]
    public class CENTROSAR
    {
        [Key]
        public string CODCAR { get; set; }
        public string NOMBRE { get; set; }
        public decimal TASA { get; set; }
        public string CORRELATIVO { get; set; }
        public string RUC { get; set; }
        public decimal TOPESCTR { get; set; }
        public string TIPO_SCTR { get; set; }
    }
}
