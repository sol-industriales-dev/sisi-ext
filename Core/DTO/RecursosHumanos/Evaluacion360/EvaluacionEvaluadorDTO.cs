using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Evaluacion360
{
    public class EvaluacionEvaluadorDTO
    {
        #region SQL
        public int id { get; set; }
        public int idPeriodo { get; set; }
        public int idPersonalEvaluado { get; set; }
        public int idPersonalEvaluador { get; set; }
        public int idCuestionario { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public int tipoRelacion { get; set; }
        public DateTime fechaLimiteEvaluacion { get; set; }
        public int cantReactivos { get; set; }
        public int cantAvance { get; set; }
        public string nombreCuestionario { get; set; }
        public string nombreCompleto { get; set; }
        public string relacion { get; set; }
        public string avance { get; set; }
        public int idEvaluacion { get; set; }
        public bool cuestionarioTerminado { get; set; }
        public string nombrePeriodo { get; set; }
        #endregion
    }
}
