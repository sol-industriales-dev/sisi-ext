using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Evaluacion360
{
    public class EstatusEvaluadorDTO
    {
        #region ADICIONAL
        public int idPeriodo { get; set; }
        public int numRegistro { get; set; }
        public string nombreEvaluador { get; set; }
        public string nombreEvaluado { get; set; }
        public string descripcionTipoRelacion { get; set; }
        public string nombreCuestionario { get; set; }
        public string estatusAvance { get; set; }
        public int idPersonalEvaluado { get; set; }
        public int idPersonalEvaluador { get; set; }
        public int idCuestionario { get; set; }
        public int cantNoIniciadas { get; set; }
        public int cantEnProceso { get; set; }
        public int cantContestadas { get; set; }
        public int idEmpresa { get; set; }
        #endregion
    }
}