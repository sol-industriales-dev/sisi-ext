using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_CheckList
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int areaId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int? usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("areaId")]
        public virtual tbl5s_Area area { get; set; }

        [ForeignKey("checkListId")]
        public virtual List<tbl5s_CC> ccs { get; set; }

        [ForeignKey("checkListId")]
        public virtual List<tbl5s_LiderArea> lideres { get; set; }

        [ForeignKey("checkListId")]
        public virtual List<tbl5s_Inspeccion> inspecciones { get; set; }
    }
}
