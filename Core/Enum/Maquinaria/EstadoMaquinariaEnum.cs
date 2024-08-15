using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum EstadoMaquinariaEnum
    {
        [DescriptionAttribute("LIBRE")]
        LIBRE = 0,
        [DescriptionAttribute("ASIGNADO")]
        ASIGNADO = 1,
        [DescriptionAttribute("TRANSITO")]
        TRANSITO = 2,
        [DescriptionAttribute("PROGRAMADA")]
        PROGRAMADA = 3,
        [DescriptionAttribute("REASIGNADADA")]
        REASIGNADADA = 4


    }
}
