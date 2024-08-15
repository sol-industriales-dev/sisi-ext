using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.PlantillasCatalogos
{
    [Table("RTPS_REGIMEN_LABORAL")]
    public class RTPS_REGIMEN_LABORAL
    {
        [Key]
        public string CODIGO_REGIMEN_LABORAL { get; set; }
        public string DESCRIPCION { get; set; }
        public string ABREV { get; set; }
        public string PRIVADO { get; set; }
        public string PUBLICO { get; set; }
        public string OTROS { get; set; }
    }
}