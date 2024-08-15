using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Overhaul
{
    public enum MotivoFallaCompEnum
    {
        [DescriptionAttribute("Vida Útil")]
        VIDA_UTIL = 0,
        [DescriptionAttribute("Estrategia")]
        ESTRATEGIA = 2,
        [DescriptionAttribute("Falla")]
        FALLA = 1
    }
}
