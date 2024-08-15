using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlAutorizacionAditiva
    {
        public int id { get; set; }
        public int aditivaId { get; set; }
        public int autorizanteId { get; set; }
        public string firma { get; set; }
        public bool rechazada { get; set; }
        public bool registroActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set;}
        public int usuarioCreacionId { get; set; }
        public int usuarioModificacionId { get; set; }

        [ForeignKey("aditivaId")]
        public tblAF_CtrlAditiva aditiva { get; set; }

        [ForeignKey("autorizanteId")]
        public tblP_Usuario autorizante { get; set; }

        [ForeignKey("usuarioCreacionId")]
        public tblP_Usuario usuarioCreacion { get; set; }

        [ForeignKey("usuarioModificacionId")]
        public tblP_Usuario usuarioModificacion { get; set; }
    }
}
