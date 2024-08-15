using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    public enum MainContextEnum
    {
        [DescriptionAttribute("MainContext")]
        Construplan = 1,
        [DescriptionAttribute("MainContextArrendadora")]
        Arrendadora = 2,
        [DescriptionAttribute("MainContextColombia")]
        Colombia = 3,
        [DescriptionAttribute("MainContextEICI")]
        EICI = 4,
        [DescriptionAttribute("MainContextIntegradora")]
        INTEGRADORA = 5,
        [DescriptionAttribute("MainContextPeru")]
        PERU = 6,
        [DescriptionAttribute("MainContextPeruStarSoft")]
        PERUSTARSOFT = 7,
        [DescriptionAttribute("MainContextGCPLAN")]
        GCPLAN = 8,
        [DescriptionAttribute("MainContextRH")]
        RHConstruplan = 5,
        [DescriptionAttribute("MainContextSigoplan")]
        SUBCONTRATISTAS_GESTOR = 9,
        [DescriptionAttribute("MainContextPruebas")]
        PRUEBAS = 11
    }
}
