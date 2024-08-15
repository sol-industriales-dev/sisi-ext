using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Evaluacion360
{
    public class tblRH_Eval360_RelacionesDet
    {
        #region SQL
        public int id { get; set; }
        public int idRelacion { get; set; }
        public int idPersonalEvaluador { get; set; }
        public int tipoRelacion { get; set; }
        public int idCuestionario { get; set; }
        public bool seEnvioCorreo { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
