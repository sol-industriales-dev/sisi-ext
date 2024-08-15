using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Evaluacion360
{
    public class RelacionDTO
    {
        #region SQL
        public int id { get; set; }
        public int idPeriodo { get; set; }
        public int idPersonalEvaluado { get; set; }
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
        public string nombreEvaluado { get; set; }
        public string nombreAutoevaluacion { get; set; }
        public string nombrePares { get; set; }
        public string nombreClientesInternos { get; set; }
        public string nombreColaboradores { get; set; }
        public string nombreJefes { get; set; }
        public int idCuestionario { get; set; }
        public string nombreCuestionario { get; set; }
        public int idPersonalEvaluador { get; set; }
        public string lstEvaluadores_PARES { get; set; }
        public string lstEvaluadores_CLIENTES_INTERNOS { get; set; }
        public string lstEvaluadores_COLABORADORES { get; set; }
        public string lstEvaluadores_JEFE { get; set; }
        public List<int> lstPersonalID { get; set; }
        public bool registroConExito { get; set; }
        public int idEmpresa { get; set; }
        #endregion
    }
}
