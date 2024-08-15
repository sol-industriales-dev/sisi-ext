using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.DTO.ControlObra
{
    public class EvaluacionAsignacionDTO
    {
        public int id { get; set; }
        public int asignacion_id { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public TipoEvaluacionEnum tipo { get; set; }
        public string tipoDesc { get; set; }
        public EstatusEvaluacionEnum estatus { get; set; }
        public string estatusDesc { get; set; }
    }
}
