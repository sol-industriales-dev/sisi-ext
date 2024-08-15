using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Evaluacion360
{
    public enum NivelAccesoEnum
    {
        [DescriptionAttribute("REGULAR")]
        REGULAR = 0,
        [DescriptionAttribute("ADMINISTRADOR")]
        ADMINISTRADOR = 1
    }
}
