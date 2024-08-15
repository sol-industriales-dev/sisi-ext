using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.Entity.ControlObra
{
    public class tblCOES_Evidencia
    {
        public int id { get; set; }
        public int evaluacion_id { get; set; }
        public int requerimiento_id { get; set; }
        public string rutaArchivo { get; set; }
        public TipoEvidenciaEnum tipo { get; set; }
        public int evidenciaInicial_id { get; set; }
        public EstatusEvidenciaEnum estatus { get; set; }
        public decimal calificacion { get; set; }
        public int ponderacion { get; set; }
        public string comentarioEvaluacion { get; set; }
        public string planAccion { get; set; }
        public string responsable { get; set; }
        public DateTime? fechaCompromiso { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
