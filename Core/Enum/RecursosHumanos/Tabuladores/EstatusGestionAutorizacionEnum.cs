using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Tabuladores
{
    public enum EstatusGestionAutorizacionEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        PENDIENTE = 0,
        [DescriptionAttribute("AUTORIZADO")]
        AUTORIZADO = 1,
        [DescriptionAttribute("RECHAZADO")]
        RECHAZADO = 2
    }
}
