using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.SeguimientoAcuerdos
{
    public enum SeguimientoAcuerdo_ReportesEnum
    {
        [DescriptionAttribute("Actividades pendientes")]
        ACTIVIDADES_PENDIENTES = 1,
        [DescriptionAttribute("Actividades finalizadas")]
        ACTIVIDADES_FINALIZADAS = 2,
        [DescriptionAttribute("Minutas con actividades pendientes")]
        MINUTA_CON_ACTIVIDAD_PENDIENTE = 3,
        [DescriptionAttribute("Minutas con actividades finalizadas")]
        MINUTA_CON_ACTIVIDAD_FINALIZADAS = 4,
        [DescriptionAttribute("Estadistico de minutas por departamento")]
        ESTADISTICO_POR_DEPARTAMENTO = 5
    }
}
