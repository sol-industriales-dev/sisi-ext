using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Mural
{
    public class tblRH_Mural
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Color { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool Estatus { get; set; }
        public int IdUsuarioCreacion { get; set; }

        [ForeignKey("IdUsuarioCreacion")]
        public virtual tblP_Usuario UsuarioCreacion { get; set; }

        [ForeignKey("IdMural")]
        public virtual List<tblRH_Mural_PostIt> PostItList { get; set; }

        [ForeignKey("IdMural")]
        public virtual List<tblRH_Mural_Seccion> SeccionList { get; set; }
    }
}
