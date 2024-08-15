using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Compras
{
    public enum TipoComprasResetEnum
    {
        [DescriptionAttribute("NO ESPECIFICADO")]
        noEspecificado = 0,
        [DescriptionAttribute("COMPRAS IMPRESAS")]
        comprasImpresas = 1,
        [DescriptionAttribute("COMPRAS NO IMPRESAS")]
        comprasNoImpresas = 2
    }
}
