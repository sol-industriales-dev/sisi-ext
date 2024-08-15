using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum periodoAvanceEnum
    {
        [DescriptionAttribute("Diario")]
        Diario = 1,
        [DescriptionAttribute("Semanal")]
        Semanal = 2
    }
}
