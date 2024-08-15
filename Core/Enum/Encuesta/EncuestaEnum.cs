using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Encuesta
{
    public enum EncuestaEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        PENDIENTE = 1,
        [DescriptionAttribute("AUTORIZADA")]
        AUTORIZADA = 2,
        [DescriptionAttribute("RECHAZADA")]
        RECHAZADA = 3
    }
}
