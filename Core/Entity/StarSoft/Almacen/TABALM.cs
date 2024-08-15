using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Almacen
{
    [Table("TABALM")]
    public class TABALM
    {
        [Key]
        public string TAALMA { get; set; }
        public string TADESCRI { get; set; }
        public string TADISTRI { get; set; }
        public string TATELEFO { get; set; }
        public string TACTLNUM { get; set; }
        public int? TANUMENT { get; set; }
        public int? TANUMSAL { get; set; }
        public int? TANUMFAC { get; set; }
        public int? TANUMNC { get; set; }
        public int? TANUMND { get; set; }
        public string TAFORMATO { get; set; }
        public string TADIRECC { get; set; }
        public string ATIPO { get; set; }
        public string TIPO_ALMACEN { get; set; }
        public bool TERCERO { get; set; }
        public bool TAPRINCIPAL { get; set; }
        public string UBIGEO { get; set; }
        public bool? FLG_ECOMMERCE { get; set; }
        public string TACOD_ESTABLECIMIENTO { get; set; }

    }
}
