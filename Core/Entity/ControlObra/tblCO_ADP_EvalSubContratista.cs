using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_ADP_EvalSubContratista
    {
        public int id { get; set; }
        public int idSubConAsignacion { get; set; }
        public string idAreaCuenta { get; set; }
        public int idSubContratista { get; set; }
        public int tipoEvaluacion { get; set; }
        public int Calificacion { get; set; }
        public string firmaAutorizacion { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public string usuarioAutorizacion { get; set; }
        public bool evaluacionPendiente { get; set; }

        [ForeignKey("idSubConAsignacion")]
        public virtual tblCO_ADP_EvalSubConAsignacion evaluacionConAsignacion { get; set; }

        [ForeignKey("idEvaluacion")]
        public virtual List<tblCO_ADP_EvalSubContratistaDet> detalleEvaluaciones { get; set; }
    }
}
