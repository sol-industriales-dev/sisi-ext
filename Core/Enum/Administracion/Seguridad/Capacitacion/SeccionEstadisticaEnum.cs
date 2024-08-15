using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum SeccionEstadisticaEnum
    {
        [DescriptionAttribute("% CAPACITACIONES OPERATIVAS")]
        PORCENTAJE_CAPACITACION = 1,
        [DescriptionAttribute("EFECTIVIDAD CICLOS OPERATIVOS")]
        EFECTIVIDAD_CICLOS = 2,
        [DescriptionAttribute("RECORRIDOS OPERATIVOS")]
        RECORRIDOS_OPERATIVOS = 3,
        [DescriptionAttribute("IMPLEMENTACIÓN 5'S")]
        IMPLEMENTACION_5S = 4
    }
}
