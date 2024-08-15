using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Contabilidad
{
    public enum TipoOperacionEnum
    {
        [DescriptionAttribute("Suma")]
        suma = 1,
        [DescriptionAttribute("Resta")]
        resta = 2,
        [DescriptionAttribute("Total")]
        total = 3
    }
}
