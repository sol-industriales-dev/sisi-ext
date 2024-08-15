using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_Institucion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool esPQ { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModificacionId { get; set; }

        [ForeignKey("UsuarioCreacionId")]
        public virtual tblP_Usuario UsuarioCreacion { get; set; }
        [ForeignKey("UsuarioModificacionId")]
        public virtual tblP_Usuario UsuarioModificacion { get; set; }
    }
}