using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Facultamiento
{
    public enum StAutorizacionEnum
    {
        [DescriptionAttribute("En espera")]
        EnEspera = 1,
        [DescriptionAttribute("Autorizó")]
        Autorizo = 2,
        [DescriptionAttribute("Rechazo")]
        Rechazo = 3,
    }
}
