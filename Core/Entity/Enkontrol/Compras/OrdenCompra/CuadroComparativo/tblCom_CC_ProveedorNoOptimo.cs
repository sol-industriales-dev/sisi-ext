using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    public class tblCom_CC_ProveedorNoOptimo
    {
        public int Id { get; set; }
        public int CalificacionId { get; set; }
        public int NumeroCompra { get; set; }
        public bool VoBo { get; set; }
        public int IdTipoCalificacion { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estatus { get; set; }
        public int UsuarioIdVoBo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? IdPartida { get; set; }

        [ForeignKey("CalificacionId")]
        public virtual tblCom_CC_Calificacion Calificacion { get; set; }

        [ForeignKey("IdTipoCalificacion")]
        public virtual tblCom_CC_TipoCalificacion TipoCalificacion { get; set; }

        [ForeignKey("UsuarioIdVoBo")]
        public virtual tblP_Usuario Usuario { get; set; }

        public int idUsuario { get; set; }

        [ForeignKey("IdPartida")]
        public virtual tblCom_CC_CalificacionPartida partida { get; set; }
    }
}
