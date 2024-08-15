using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Evaluacion360
{
    public class CuestionarioDetDTO
    {
        #region SQL
        public int id { get; set; }
        public int idCuestionario { get; set; }
        public int idConducta { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public int idGrupo { get; set; }
        public int idCompetencia { get; set; }
        public string nombreCuestionario { get; set; }
        public string descripcionConducta { get; set; }
        public string nombreCompetencia { get; set; }
        public string definicion { get; set; }
        public string nombreGrupo { get; set; }
        public List<int> lstConductasID { get; set; }
        #endregion
    }
}