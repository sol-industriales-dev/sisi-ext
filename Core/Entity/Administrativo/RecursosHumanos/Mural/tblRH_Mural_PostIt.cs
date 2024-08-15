using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Mural
{
    public class tblRH_Mural_PostIt
    {
        public int Id { get; set; }
        public int IdMural { get; set; }
        public string Texto { get; set; }
        public int PosicionX { get; set; }
        public int PosicionY { get; set; }
        public string ColorFondo { get; set; }
        public int Altura { get; set; }
        public int Ancho { get; set; }
        public int? IdSeccion { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int IdUsuarioCreacion { get; set; }

        [ForeignKey("IdMural")]
        public virtual tblRH_Mural Mural { get; set; }
        
        [ForeignKey("IdSeccion")]
        public virtual tblRH_Mural_Seccion Seccion { get; set; }
        
        [ForeignKey("IdUsuarioCreacion")]
        public virtual tblP_Usuario UsuarioCreacion { get; set; }
    }
}
