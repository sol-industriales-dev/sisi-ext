using Core.Entity.Principal.Configuraciones;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Sistema_Contable.iTiposMovimientos
{
    public class tblC_TM_Relitm : RegistroDTO
    {
        public int Id { get; set; }
        public EmpresaEnum PalEmpresa { get; set; }
        public string PalSistema { get; set; }
        public string PaliTmPeru { get; set; }
        public int PaliTm { get; set; }
        public EmpresaEnum SecEmpresa { get; set; }
        public string SecSistema { get; set; }
        public int SeciTm { get; set; }
    }
}
