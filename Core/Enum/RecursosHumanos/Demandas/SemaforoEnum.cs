using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Demandas
{
    public enum SemaforoEnum
    {
        [DescriptionAttribute("Verde")]
        VERDE = 1,
        [DescriptionAttribute("Ambar")]
        AMBAR = 2,
        [DescriptionAttribute("Rojo")]
        ROJO = 3
    }
}
