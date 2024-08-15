using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.CadenaProductiva
{
    public enum BancoEnum
    {
        [DescriptionAttribute("BANORTE")]
        BANORTE = 3217,
        [DescriptionAttribute("BANAMEX")]
        BANAMEX = 6544,
        [DescriptionAttribute("SCOTIABANK")]
        SCOTIABANK = 32046,
        [DescriptionAttribute("MONEX")]
        MONEX = 1097745,
    }
}
