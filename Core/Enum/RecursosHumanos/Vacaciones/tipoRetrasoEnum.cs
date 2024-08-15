using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Vacaciones
{
    public enum TipoRetrasoEnum
    {
        [DescriptionAttribute("Justificacion de incidecia mayor")]
        IncidenciaMayor = 0,
        [DescriptionAttribute("Permiso de salida durante la jornada laboral")]
        PermisoSalida = 1,
    }
}
