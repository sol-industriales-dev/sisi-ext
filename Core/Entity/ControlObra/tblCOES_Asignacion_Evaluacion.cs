using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.Entity.ControlObra
{
    public class tblCOES_Asignacion_Evaluacion
    {
        public int id { get; set; }
        public int asignacion_id { get; set; }
        public DateTime fecha { get; set; }
        public TipoEvaluacionEnum tipo { get; set; }
        public EstatusEvaluacionEnum estatus { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
