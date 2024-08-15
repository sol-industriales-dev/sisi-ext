using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Vacaciones
{
    public enum GestionEstatusEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        PENDIENTE = 3,
        [DescriptionAttribute("AUTORIZADO")]
        AUTORIZADO = 1,
        [DescriptionAttribute("RECHAZADO")]
        RECHAZADO = 2
    }
}
