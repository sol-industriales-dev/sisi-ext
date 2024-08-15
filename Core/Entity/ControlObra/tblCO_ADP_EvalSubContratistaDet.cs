using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_ADP_EvalSubContratistaDet
    {
        public int id { get; set; }
        public int idEvaluacion { get; set; }
        public int tipoEvaluacion { get; set; }
        public int idRow { get; set; }
        public string rutaArchivo { get; set; }
        public DateTime fechaDocumento { get; set; }
        public string comentario { get; set; }
        public string planesDeAccion { get; set; }
        public string responsable { get; set; }
        public string fechaCompromiso { get; set; }
        public int calificacion { get; set; }
        public bool evaluacionPendiente { get; set; }
        public bool opcional { get; set; }

        [ForeignKey("idEvaluacion")]
        public virtual tblCO_ADP_EvalSubContratista evaluacionSubcontratista { get; set; }
    }
}
