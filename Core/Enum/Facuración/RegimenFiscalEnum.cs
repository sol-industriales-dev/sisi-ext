using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Facuración
{
    public enum RegimenFiscalEnum
    {
        [DescriptionAttribute("General de ley")]
        GeneralDeLey = 1,
        [DescriptionAttribute("REGIMEN GENERAL DE LEY DE PERSONAS MORALES")]
        GeneralMorales = 2
    }
}
