using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.PlantillasCatalogos
{
    [Table("RTPS_REGIMEN_ESSALUD")]
    public class RTPS_REGIMEN_ESSALUD
    {
        [Key]
        public string CODIGO { get; set; }
        public string DESCRIPCION { get; set; }
        public string ABREV { get; set; }
    }
}