using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    public class tblCom_CC_PermisoCompradorCalificarOC
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("usuarioId")]
        public virtual tblP_Usuario comprador { get; set; }
    }
}
