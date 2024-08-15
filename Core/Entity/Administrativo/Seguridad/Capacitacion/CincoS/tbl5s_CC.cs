using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_CC
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int checkListId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int? usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("checkListId")]
        public virtual tbl5s_CheckList checkList { get; set; }

        [ForeignKey("cc5sId")]
        public virtual List<tbl5s_Calendario> calendario { get; set; }
    }
}
