using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.FlujoEfectivo
{
    public enum operadorCptoEnum
    {
        [DescriptionAttribute("=")]
        igual = 0,
        [DescriptionAttribute("+")]
        mas = 1,
        [DescriptionAttribute("-")]
        menos = 2,
    }
}
