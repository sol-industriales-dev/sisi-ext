using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;

namespace Core.Entity.Administrativo.RecursosHumanos.Mural
{
    public class tblMural_Workspace
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int usuarioID { get; set; }
        public tblP_Usuario usuario { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public string contenido { get; set; }
        public string icono { get; set; }
        public bool modificado { get; set; }
        public bool estatus { get; set; }
    }
}
