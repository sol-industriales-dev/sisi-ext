using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.ConciliacionHorometros
{
    public enum ConsideracionEnum
    {
        [DescriptionAttribute("Incluye")]
        Incluye = 1,
        [DescriptionAttribute("No Incluye")]
        NoIncluye = 2,
    }
}
