using Core.Entity.Principal.Configuraciones;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Cuentas
{
    public class tblC_Cta_RelCuentas : RegistroDTO
    {
        public int id { get; set; }
        #region Cuenta principal
        public EmpresaEnum palEmpresa { get; set; }
        public int palCta { get; set; }
        public int palScta { get; set; }
        public int palSscta { get; set; }
        #endregion
        #region Cuenta secundaria
        public EmpresaEnum secEmpresa { get; set; }
        public int secCta { get; set; }
        public int secScta { get; set; }
        public int secSscta { get; set; }
        //public int digito{ get; set; }
        #endregion
    }
}
