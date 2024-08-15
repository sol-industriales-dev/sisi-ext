using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum EstatusEvidenciaEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        PENDIENTE = 1,
        [DescriptionAttribute("APROBADO")]
        APROBADO = 2,
        [DescriptionAttribute("REPROBADO")]
        REPROBADO = 3
    }
}
