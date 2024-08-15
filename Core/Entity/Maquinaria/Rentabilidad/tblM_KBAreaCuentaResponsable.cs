using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Rentabilidad
{
    public class tblM_KBAreaCuentaResponsable
    {
        public int id { get; set; }
        public int areaCuentaID { get; set; }
        public int usuarioResponsableID { get; set; }
        public bool estatus { get; set; }
        public virtual tblM_KBUsuarioResponsable usuarioResponsable { get; set; }
        public virtual tblP_CC areaCuenta { get; set; }
    }
}
