using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.MAZDA
{
    public enum PeriodoEnum
    {
        [DescriptionAttribute("MENSUAL")]
        MENSUAL = 1,
        [DescriptionAttribute("BIMESTRAL")]
        BIMESTRAL = 2,
        [DescriptionAttribute("TRIMESTRAL")]
        TRIMESTRAL = 3,
        [DescriptionAttribute("SEMESTRAL")]
        SEMESTRAL = 4,
        [DescriptionAttribute("ANUAL")]
        ANUAL = 5
    }
}
