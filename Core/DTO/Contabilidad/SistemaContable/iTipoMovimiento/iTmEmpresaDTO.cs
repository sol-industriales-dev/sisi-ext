using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.SistemaContable.iTipoMovimiento
{
    public class iTmEmpresaDTO
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Prefijo { get; set; }
        public EmpresaEnum Empresa { get; set; }
    }
}
