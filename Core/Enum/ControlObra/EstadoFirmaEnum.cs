using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum EstadoFirmaEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        PENDIENTE = 1,
        [DescriptionAttribute("AUTORIZADO")]
        AUTORIZADO = 2,
        [DescriptionAttribute("RECHAZADO")]
        RECHAZADO = 3
    }
}
