using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum CcOverhaul
    {
        [DescriptionAttribute("COMPRA COMPONENTES SAN AGUSTIN")]
        ComponenetesSanAgustin = 980,
        [DescriptionAttribute("COSTOS TALLER OVERHAUL LA COLORADA")]
        OverhaulColorada = 983,
        [DescriptionAttribute("COMPRA COMPONENTES NOCHEBUENA")]
        ComponenetesNochebuena = 985,
        [DescriptionAttribute("COMPRA COMPONENTES LA COLORADA")]
        ComponenetesColorada = 986,
        [DescriptionAttribute("COSTOS TALLER OVERHAUL NOCHEBUENA")]
        OverhaulNochebuena = 989,
    }
}
