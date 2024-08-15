using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Evaluacion360
{
    public class RelacionDetDTO
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

        #region ADICIONAL
        public string nombreCompleto { get; set; }
        public string nombreCuestionario { get; set; }
        public string correo { get; set; }
        public int idPeriodo { get; set; }
        public int idPersonalEvaluado { get; set; }
        #endregion
    }
}