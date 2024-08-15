using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum PonderacionCriterioEnum
    {
        [DescriptionAttribute("Bajo")]
        bajo = 1,
        [DescriptionAttribute("Medio")]
        medio = 2,
        [DescriptionAttribute("Crítico")]
        critico = 3
    }
}
