using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal.Alertas
{
    public enum AlertasEnum : int
    {
        [DescriptionAttribute("MENSAJE")]
        MENSAJE = 1,
        [DescriptionAttribute("REDIRECCION")]
        REDIRECCION = 2,
        [DescriptionAttribute("AVANZADO")]
        AVANZADO = 3,
        [DescriptionAttribute("MENSAJE CON VISTO AL DAR CLICK")]
        MENSAJE_VISTO_CLICK = 4
    }
}
