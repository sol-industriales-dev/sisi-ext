using Core.Entity.Principal.Configuraciones;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Sistema_Contable.CentroCostos
{
    public class tblC_CC_RelObras : RegistroDTO
    {
        public int Id { get; set; }
        public EmpresaEnum PalEmpresa { get; set; }
        public string PalObra { get; set; }
        public EmpresaEnum SecEmpresa { get; set; }
        public string SecObra { get; set; }
    }
}
