using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Evaluacion360
{
    public class EvaluacionEvaluadorDetDTO
    {
        #region SQL
        public int id { get; set; }
        public int idEvaluacion { get; set; }
        public int idConducta { get; set; }
        public int idCriterio { get; set; }
        public string comentario { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public int idPeriodo { get; set; }
        public int idPersonalEvaluado { get; set; }
        public int idPersonalEvaluador { get; set; }
        public string descripcionConducta { get; set; }
        public string nombreCompetencia { get; set; }
        public string btnSiguienteFinalizar { get; set; }
        public int idConductaSiguiente { get; set; }
        public int idCuestionario { get; set; }
        public int idConductaAnterior { get; set; }
        public int orden { get; set; }
        public int idGrupo { get; set; }
        public decimal limSuperior { get; set; }
        public decimal promedio { get; set; }
        public bool mostrarComentario { get; set; }
        #endregion
    }
}