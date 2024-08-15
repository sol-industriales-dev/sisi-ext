using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_InspeccionDet
    {
        public int id { get; set; }
        public int inspeccionId { get; set; }
        public int cincoId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int? usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("inspeccionId")]
        public virtual tbl5s_Inspeccion inspeccion { get; set; }

        [ForeignKey("cincoId")]
        public virtual tbl5s_5s cincoS { get; set; }
    }
}
