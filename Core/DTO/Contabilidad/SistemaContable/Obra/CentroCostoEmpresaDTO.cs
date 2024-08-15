using Core.DTO.Enkontrol;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.SistemaContable.Obra
{
    public class CentroCostoEmpresaDTO : CentroCostoDTO
    {
        public EmpresaEnum Empresa { get; set; }
    }
}
