using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.SeguimientoAcuerdos
{
    public enum PrioridadEnum
    {
        [DescriptionAttribute("Baja")]
        BAJA = 3,
        [DescriptionAttribute("Media")]
        MEDIA = 2,
        [DescriptionAttribute("Alta")]
        ALTA = 1
    }
}
