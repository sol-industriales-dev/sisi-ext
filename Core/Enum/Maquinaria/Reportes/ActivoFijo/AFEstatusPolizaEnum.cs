using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Reportes.ActivoFijo
{
    public enum AFEstatusPolizaEnum
    {
        [DescriptionAttribute("Actualizada")]
        A = 1,
        [DescriptionAttribute("Bloqueada")]
        B = 2,
        [DescriptionAttribute("Capturada")]
        C = 3,
        [DescriptionAttribute("Errónea")]
        E = 4,
        [DescriptionAttribute("Validada")]
        V = 5
    }
}