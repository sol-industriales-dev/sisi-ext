using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Cotizaciones
{
    public enum TipoMonedaEnum
    {
        [DescriptionAttribute("MXN")]
        MXN = 1,
        [DescriptionAttribute("USD")]
        USD = 2,
        [DescriptionAttribute("COP")]
        COP = 3,
        [DescriptionAttribute("EUR")]
        EUR = 4,
        [DescriptionAttribute("SOL")]
        SOL = 5,
    }
}
