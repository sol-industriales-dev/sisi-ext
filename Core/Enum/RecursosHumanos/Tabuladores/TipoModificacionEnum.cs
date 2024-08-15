using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Tabuladores
{
    public enum TipoModificacionEnum
    {
        [DescriptionAttribute("INCREMENTO A EMPLEADOS ACTIVOS")]
        INCREMENTO_ANUAL_A_EMPLEADOS_ACTIVOS = 1,
        [DescriptionAttribute("MODIFICACION A PUESTOS")]
        MODIFICACION_A_PUESTOS = 2
    }
}
