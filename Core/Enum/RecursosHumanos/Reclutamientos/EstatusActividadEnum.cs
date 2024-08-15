using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Reclutamientos
{
    public enum EstatusActividadEnum
    {
        [DescriptionAttribute("NO APROBADO")]
        noAprobado = 0,
        [DescriptionAttribute("APROBADO")]
        aprobado = 1,
        [DescriptionAttribute("PENDIENTE")]
        pendiente = 2,
        [DescriptionAttribute("NO APLICA")]
        noAplica = 3
    }
}
