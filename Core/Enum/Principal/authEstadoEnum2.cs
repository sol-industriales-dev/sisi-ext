using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    public enum authEstadoEnum2
    {
        [DescriptionAttribute("En Espera")]
        EnEspera = 0,
        [DescriptionAttribute("Autorizado")]
        Autorizado = 1,
        [DescriptionAttribute("Rechazado")]
        Rechazado = 2
    }
}
