using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Multiempresa
{
    public enum  EmpresaEnum
    {
        [DescriptionAttribute("CONSTRUPLAN")]
        Construplan = 1,
        [DescriptionAttribute("ARRENDADORA")]
        Arrendadora = 2,
        [DescriptionAttribute("COLOMBIA")]
        Colombia = 3,
        [DescriptionAttribute("EICI")]
        EICI = 4,
        [DescriptionAttribute("INTEGRADORA")]
        Integradora = 5,
        [DescriptionAttribute("PERU")]
        Peru = 6,
        [DescriptionAttribute("PERÚ")]
        PeruStarSoft = 7,
        [DescriptionAttribute("GCPLAN")]
        GCPLAN = 8,
        [DescriptionAttribute("PRUEBAS")]
        Pruebas = 11
    }
}
