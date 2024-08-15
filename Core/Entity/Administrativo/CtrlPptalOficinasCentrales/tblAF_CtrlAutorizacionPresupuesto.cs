using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlAutorizacionPresupuesto
    {
        public int id { get; set; }
        public int presupuestoId { get; set; }
        public int autorizanteId { get; set; }
        public string firma { get; set; }
        public bool registroActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int usuarioModificacionId { get; set; }

        [ForeignKey("usuarioCreacionId")]
        public virtual tblP_Usuario usuarioCreacion { get; set; }

        [ForeignKey("usuarioModificacionId")]
        public virtual tblP_Usuario usuarioModificacion { get; set; }

        [ForeignKey("presupuestoId")]
        public virtual tblAF_CtrlPptalOfCe_PptoAnual presupuesto { get; set; }
    }
}
