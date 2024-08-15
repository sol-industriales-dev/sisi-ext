using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.Entity.ControlObra
{
    public class tblCOES_CambioEvaluacion
    {
        public int id { get; set; }
        public int evaluacion_id { get; set; }
        public DateTime fechaAnterior { get; set; }
        public DateTime fechaNueva { get; set; }
        public int usuarioSolicitante_id { get; set; }
        public string motivoCambio { get; set; }
        public EstatusCambioEvaluacionEnum estatus { get; set; }
        public string comentarioAutorizante { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
