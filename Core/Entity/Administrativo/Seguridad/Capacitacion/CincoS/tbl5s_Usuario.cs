using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_Usuario
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public int? privilegioId { get; set; }
        public int? areaOperativaId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int? usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("usuarioId")]
        public virtual tblP_Usuario usuario { get; set; }

        [ForeignKey("privilegioId")]
        public virtual tbl5s_Privilegio privilegio { get; set; }

        [ForeignKey("usuario5sId")]
        public virtual List<tbl5s_CC_Usuario> ccs { get; set; }

        [ForeignKey("usuario5sId")]
        public virtual List<tbl5s_LiderArea> lideresArea { get; set; }

        [ForeignKey("areaOperativaId")]
        public virtual tbl5s_AreaOperativa areaOperativa { get; set; }
    }
}
