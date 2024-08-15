using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum EstatusStandByEnum
    {
        [DescriptionAttribute("Pendiente")]
        PENDIENTE = 1,
        [DescriptionAttribute("Autorizado")]
        AUTORIZADO = 2,
        [DescriptionAttribute("Rechazado")]
        RECHAZADO = 3,
        [DescriptionAttribute("Liberado")]
        LIBERADO = 4
    }
}
