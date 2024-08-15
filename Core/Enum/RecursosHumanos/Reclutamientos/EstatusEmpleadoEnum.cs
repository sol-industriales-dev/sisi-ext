using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Reclutamientos
{
    public enum EstatusEmpleadoEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        pendiente = 1,
        [DescriptionAttribute("CONTRATADO")]
        contratado = 2,
        [DescriptionAttribute("NO CONTRATADO")]
        noContratado = 3
    }
}
