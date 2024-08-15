using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Compras.Proveedores
{
    public enum TipoProveedorEnum
    {
        [DescriptionAttribute("Moneda Nacional")]
        provNacional = 1,
        [DescriptionAttribute("Moneda Extranjera")]
        provExtranjero = 2,
    }
}
