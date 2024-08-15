using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_LiderArea
    {
        public int id { get; set; }
        public int checkListId { get; set; }
        public int usuario5sId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int? usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("checkListId")]
        public virtual tbl5s_CheckList checkList { get; set; }

        [ForeignKey("usuario5sId")]
        public virtual tbl5s_Usuario usuario { get; set; }
    }
}
