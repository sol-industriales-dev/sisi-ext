using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.BajasPersonal
{
    public enum AutorizacionEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        PENDIENTE = 0,
        [DescriptionAttribute("AUTORIZADA")]
        AUTORIZADA = 1,
        [DescriptionAttribute("RECHAZADA")]
        RECHAZADA = 2,
    }
}
