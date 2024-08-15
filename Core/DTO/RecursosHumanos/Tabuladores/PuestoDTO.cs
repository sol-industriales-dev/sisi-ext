using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class PuestoDTO
    {
        #region SQL
        public int puesto { get; set; }
        public string descripcion { get; set; }
        public string desc_corta { get; set; }
        public decimal porcentaje { get; set; }
        public string descripcion_puesto { get; set; }
        public int FK_AreaDepartamento { get; set; }
        public int FK_TipoNomina { get; set; }
        public int FK_Sindicato { get; set; }
        public int FK_NivelMando { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public int nivelMando { get; set; }
        public string descAreaDepartamento { get; set; }
        public string descNivelMando { get; set; }
        #endregion
    }
}
