using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public enum ArchivoValidacionEnum
    {
        [DescriptionAttribute("Pendiente")]
        pendiente = 0,
        [DescriptionAttribute("Validado")]
        validado = 1,
        [DescriptionAttribute("Rechazado")]
        rechazado = 2
    }
}
