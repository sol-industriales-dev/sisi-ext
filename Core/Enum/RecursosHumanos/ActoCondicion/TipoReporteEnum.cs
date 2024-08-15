using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.ActoCondicion
{
    public enum TipoReporteEnum
    {
        [DescriptionAttribute("Amonestación")]
        AMONESTACION = 1,
        [DescriptionAttribute("Suspensión")]
        SUSPENSION = 2,
        [DescriptionAttribute("Carta de responsabilidad")]
        CARTA_RESPONSABILIDAD = 3,
        [DescriptionAttribute("Acta administrativa")]
        ACTA_ADMINISTRATIVA = 4
    }
}
