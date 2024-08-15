using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Vacaciones
{
    public enum Incapacidades2Enum
    {
        [DescriptionAttribute("Inicial")]
        Inicial = 0,
        [DescriptionAttribute("Subsecuente")]
        Subsecuente = 1,
    }
}
