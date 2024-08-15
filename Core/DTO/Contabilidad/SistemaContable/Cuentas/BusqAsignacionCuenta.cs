using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.SistemaContable.Cuentas
{
    public class BusqAsignacionCuenta
    {
        public EmpresaEnum palEmpresa { get; set; }
        public EmpresaEnum secEmpresa { get; set; }
    }
}
