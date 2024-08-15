using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Mural
{
    public class tblMural_Workspace_Members
    {
        public int id { get; set; }
        public int workSpaceID { get; set; }
        //public tblMural_Workspace workSpace { get; set; }
        public string usuarioNombre { get; set; }
        public int usuarioID { get; set; }
        //public tblP_Usuario usuario { get; set; }
        public int tipo { get; set; }
        public bool estatus { get; set; }
    }
}
