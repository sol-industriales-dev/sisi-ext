using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.BajasPersonal
{
    public enum PrioridadEnum
    {
        [DescriptionAttribute("Alta")]
        alta = 1,
        [DescriptionAttribute("Media")]
        media = 2,
        [DescriptionAttribute("Baja")]
        baja = 3
    }
}
