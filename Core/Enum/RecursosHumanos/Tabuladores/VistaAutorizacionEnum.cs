using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Tabuladores
{
    public enum VistaAutorizacionEnum
    {
        [DescriptionAttribute("ASIGNACIÓN TABULADORES")]
        ASIGNACION_TABULADORES = 0,
        [DescriptionAttribute("MODIFICACIÓN TABULADORES")]
        MODIFICACION_TABULADORES = 1,
        [DescriptionAttribute("PLANTILLAS PERSONAL")]
        PLANTILLAS_PERSONAL = 2,
        [DescriptionAttribute("REPORTES TABULADORES")]
        REPORTES_TABULADORES = 3,
    }
}
