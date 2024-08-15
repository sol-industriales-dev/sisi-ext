using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum AspectoEvaluadoCriterioEnum
    {
        [DescriptionAttribute("N/A")]
        NA = 0,
        [DescriptionAttribute("Comportamiento Seguro")]
        COMPORTAMIENTO_SEGURO = 1,
        [DescriptionAttribute("Técnica Operacional")]
        TECNICA_OPERACIONAL = 2,
        [DescriptionAttribute("Mantenimiento Técnico")]
        MANTENIMIENTO_TECNICO = 3
    }
}
