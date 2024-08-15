using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Puestos
    {
        [Key]
        public int puesto { get; set; }
        public string descripcion { get; set; }
        public string desc_corta { get; set; }
        public decimal? porcentaje { get; set; }
        public string descripcion_puesto { get; set; }
        public int FK_AreaDepartamento { get; set; }
        public int FK_TipoNomina { get; set; }
        public int FK_Sindicato { get; set; }
        public int FK_NivelMando { get; set; }
        public decimal BAE { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int? FK_UsuarioModificacion { get; set; }
        public bool esEvaluacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public int? puestoNuevo { get; set; }

        //[ForeignKey("FK_TipoNomina")]
        //public virtual tblRH_EK_Tipos_Nomina virtualTipoNomina { get; set; }
    }
}
