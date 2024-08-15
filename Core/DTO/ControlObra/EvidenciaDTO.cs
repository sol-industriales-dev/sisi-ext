using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.DTO.ControlObra
{
    public class EvidenciaDTO
    {
        public int evidencia_id { get; set; }
        public int evaluacion_id { get; set; }
        public int requerimiento_id { get; set; }
        public string rutaArchivo { get; set; }
        public TipoEvidenciaEnum tipo { get; set; }
        public string tipoDesc { get; set; }
        public int evidenciaInicial_id { get; set; }
        public EstatusEvidenciaEnum estatus { get; set; }
        public string estatusDesc { get; set; }
        public decimal calificacion { get; set; }
        public int ponderacion { get; set; }
        public string comentarioEvaluacion { get; set; }
        public string planAccion { get; set; }
        public string responsable { get; set; }
        public DateTime? fechaCompromiso { get; set; }
        public string fechaCompromisoString { get; set; }
    }
}
