using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.ActoCondicion
{
    public enum AccionInfraccionEnum
    {
        [DescriptionAttribute("AMONESTACIÓN")]
        AMONESTACION = 1,
        [DescriptionAttribute("ACTA ADMINISTRATIVA")]
        ACTA_ADMINISTRATIVA = 2,
        [DescriptionAttribute("ACTA ADMINISTRATIVA CON SUSPENSIÓN")]
        SUSPENSION = 3,
        [DescriptionAttribute("RESCISIÓN DE CONTRATO")]
        RESCISION = 4
    }
}
