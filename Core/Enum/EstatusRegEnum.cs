using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public enum EstatusRegEnum : int
    {
        [DescriptionAttribute("Pendiente")]
        PENDIENTE = 1,
        [DescriptionAttribute("Autorizados")]
        AUTORIZADO = 2,
        [DescriptionAttribute("Rechazado")]
        RECHAZADO = 3,
        [DescriptionAttribute("Eliminado")]
        ELIMINADO = 4
    }
}
