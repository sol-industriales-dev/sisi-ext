using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Contabilidad.Nomina
{
    public enum TipoPeriodoEnum
    {
        [DescriptionAttribute("Semanal")]
        Semanal = 1,
        [DescriptionAttribute("Quincenal")]
        Quincenal = 4,
        [DescriptionAttribute("Aguinaldo")]
        Aguinaldo = 10
    }
}
