using Core.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_CatalogoCC : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public int clasificacionCcId { get; set; }
        public int? area { get; set; }
        public int? cuenta { get; set; }
        public bool semanal { get; set; }
        public bool quincenal { get; set; }
        public bool depositoSindicato { get; set; }
        public decimal porcentajeSindicato { get; set; }

        [ForeignKey("clasificacionCcId")]
        public virtual tblC_Nom_ClasificacionCC clasificacion { get; set; }
    }
}
