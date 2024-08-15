using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Compras
{
    public enum TipoCentroCostoEnum
    {
        [DescriptionAttribute("ADMINISTRATIVO")]
        ADMINISTRATIVO = 1,
        [DescriptionAttribute("OPERATIVO")]
        OPERATIVO = 2
    }
}
