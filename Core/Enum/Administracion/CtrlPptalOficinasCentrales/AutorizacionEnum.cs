using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.CtrlPptalOficinasCentrales
{
    public enum AutorizacionEnum
    {
        [DescriptionAttribute("RECHAZADO")]
        rechazado = 0,
        [DescriptionAttribute("AUTORIZADO")]
        autorizado = 1,
        [DescriptionAttribute("PENDIENTE")]
        pendiente = 2
    }
}
