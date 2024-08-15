using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Evaluacion360
{
    public class tblRH_Eval360_EvaluacionesEvaluadorDet
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
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
