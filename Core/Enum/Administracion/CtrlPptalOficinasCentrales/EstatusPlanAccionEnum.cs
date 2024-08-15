using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.CtrlPptalOficinasCentrales
{
    public enum EstatusPlanAccionEnum
    {
        [DescriptionAttribute("ABIERTO")]
        abierto = 1,
        [DescriptionAttribute("PENDIENTE")]
        pendiente = 2,
        [DescriptionAttribute("RETRASADO")]
        retrasado = 3,
        [DescriptionAttribute("CERRADO")]
        cerrado = 4
    }
}
