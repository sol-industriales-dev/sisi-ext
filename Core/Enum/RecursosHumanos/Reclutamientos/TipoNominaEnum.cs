using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Reclutamientos
{
    public enum TipoNominaEnum
    {
        [DescriptionAttribute("SEMANAL")]
        semanal = 1,
        [DescriptionAttribute("QUINCENAL")]
        quincenal = 4
    }
}