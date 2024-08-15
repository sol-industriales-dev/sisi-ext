using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Capacitacion
{
    public enum MandoEnum
    {
        [DescriptionAttribute("Mando Alto")]
        ALTO = 1,
        [DescriptionAttribute("Mando Medio")]
        MEDIO = 2,
        [DescriptionAttribute("Mando Técnico-Admón")]
        TECNICO_ADMON = 3
    }
}
