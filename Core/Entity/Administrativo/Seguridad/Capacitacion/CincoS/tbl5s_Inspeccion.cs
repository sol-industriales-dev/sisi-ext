using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_Inspeccion
    {
        public int id { get; set; }
        public int checkListId { get; set; }
        public string inspeccion { get; set; }
        public int subAreaId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int? usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("checkListId")]
        public virtual tbl5s_CheckList checkList { get; set; }

        [ForeignKey("subAreaId")]
        public virtual tbl5s_SubArea subArea { get; set; }

        [ForeignKey("inspeccionId")]
        public virtual List<tbl5s_InspeccionDet> detalles { get; set; }
    }
}
