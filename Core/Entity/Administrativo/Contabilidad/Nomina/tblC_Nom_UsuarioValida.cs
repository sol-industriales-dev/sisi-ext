using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_UsuarioValida
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("usuarioId")]
        public virtual tblP_Usuario usuario { get; set; }
    }
}
