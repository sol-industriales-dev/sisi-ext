using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    public enum authEstadoEnum
    {
        [DescriptionAttribute("En Espera")]
        EnEspera = 0,
        [DescriptionAttribute("Autorizado")]
        Autorizado = 1,
        [DescriptionAttribute("Rechazado")]
        Rechazado = 2,
        [DescriptionAttribute("Autorizando")]
        EnTurno = 3,
        [DescriptionAttribute("Sólo Pendiente VoBo")]
        SoloPendienteVoBo = 4,
        [DescriptionAttribute("Sólo Pendiente Autorización")]
        SoloPendienteAutorizacion = 5,
    }
}
