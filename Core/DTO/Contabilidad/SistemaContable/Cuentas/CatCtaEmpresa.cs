using Core.DTO.Contabilidad.Poliza;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.SistemaContable.Cuentas
{
    public class CatCtaEmpresa : CatctaDTO
    {
        public EmpresaEnum empresa { get; set; }
    }
}
