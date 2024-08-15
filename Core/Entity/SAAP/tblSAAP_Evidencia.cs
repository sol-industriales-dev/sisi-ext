using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SAAP
{
    public class tblSAAP_Evidencia
    {
        public int id { get; set; }
        public int agrupacion_id { get; set; }
        public int area { get; set; }
        public int actividad_id { get; set; }
        public int consecutivo { get; set; }
        public string rutaEvidencia { get; set; }
        public decimal progresoEstimado { get; set; }
        public string comentariosCaptura { get; set; }
        public int usuarioEvaluador_id { get; set; }
        public string comentariosEvaluador { get; set; }
        public decimal progreso { get; set; }
        public bool terminacion { get; set; }
        public DateTime? fechaEvaluacion { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
