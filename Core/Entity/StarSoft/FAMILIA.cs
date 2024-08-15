using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft
{
    [Table("FAMILIA")]
    public class FAMILIA
    {
        [Key]
        public string FAM_CODIGO { get; set; }
        public string FAM_NOMBRE { get; set; }
        public string FAM_CTA { get; set; }
        public string FAM_DEBE { get; set; }
        public string FAM_HABER { get; set; }
        public string FAM_COMPRA { get; set; }
        public string FAM_EXISTENCIA { get; set; }
        public string DMOV_CENCO { get; set; }
        public decimal LIMITEDSCTO { get; set; }
        public bool DIVERSOS { get; set; }
        public string FAM_DEBE_AC { get; set; }
        public string FAM_HABER_AC { get; set; }
        public bool NO_GIRO_NEGOCIO { get; set; }
        public bool? FLG_ECOMMERCE { get; set; }
    }
}
