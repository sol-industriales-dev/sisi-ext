using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Rentabilidad
{
    public class tblM_KBUsuarioResponsable
    {
        public int id { get; set; }
        public int usuarioResponsableID { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }

        public virtual List<tblM_KBAreaCuentaResponsable> areaCuentaResponsable { get; set; }

        public virtual tblP_Usuario usuarioResponsable { get; set; }
    }
}
