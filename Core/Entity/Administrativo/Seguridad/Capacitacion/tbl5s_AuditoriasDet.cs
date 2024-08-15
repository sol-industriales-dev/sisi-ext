using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tbl5s_AuditoriasDet
    {
        #region SQL
        public int id { get; set; }
        public int auditoriaId { get; set; }
        public int inspeccionId { get; set; }
        public string descripcion { get; set; }
        public int respuesta { get; set; }
        public string accion { get; set; }
        public int usuario5sId { get; set; }
        public DateTime? fecha { get; set; }
        public string comentarioLider { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("auditoriaId")]
        public virtual tbl5s_Auditorias auditoria { get; set; }
        #endregion
    }
}
