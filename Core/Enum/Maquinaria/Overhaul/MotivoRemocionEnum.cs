using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Overhaul
{
    public enum MotivoRemocionEnum
    {
        [DescriptionAttribute("Vida Útil")]
        VidaUtil = 0,
        [DescriptionAttribute("Falla")]
        Falla = 1,
        [DescriptionAttribute("Estrategia")]
        Estrategia = 2
    }
}