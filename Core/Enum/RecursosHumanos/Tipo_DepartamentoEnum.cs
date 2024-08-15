using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos
{
    public enum Tipo_DepartamentoEnum
    {
        [DescriptionAttribute("ADMON")]
        ADMON = 1,
        [DescriptionAttribute("OPERATIVO")]
        OPERATIVO = 2,
        [DescriptionAttribute("TALLER")]
        TALLER = 3
    }
}
