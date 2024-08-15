using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class InfoRegistroTablaDTO
    {
        public bool estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int usuarioModificacionId { get; set; }

        [ForeignKey("usuarioCreacionId")]
        public virtual tblP_Usuario usuarioCreacion { get; set; }

        [ForeignKey("usuarioModificacionId")]
        public virtual tblP_Usuario usuarioModificacion { get; set; }
    }
}
