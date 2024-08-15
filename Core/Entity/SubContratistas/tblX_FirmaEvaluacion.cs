using Core.Entity.ControlObra;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_FirmaEvaluacion
    {
        [Key]
        public int id { get; set; }
        public int evaluacionId { get; set; }
        public bool tieneTodasLasFirmas { get; set; }
        public int? firmantePendienteId { get; set; }
        public int? numeroOrdenFirmantePendiente { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int usuarioModificacionId { get; set; }

        [ForeignKey("evaluacionId")]
        public virtual tblCO_ADP_EvalSubConAsignacion evaluacion { get; set; }

        [ForeignKey("firmantePendienteId")]
        public virtual tblX_Firmante firmantePendiente { get; set; }

        [ForeignKey("usuarioCreacionId")]
        public virtual tblP_Usuario usuarioCreacion { get; set; }

        [ForeignKey("usuarioModificacionId")]
        public virtual tblP_Usuario usuarioModificacion { get; set; }

        [ForeignKey("firmaEvaluacionId")]
        public virtual List<tblX_FirmaEvaluacionDetalle> detalle { get; set; }
    }
}
