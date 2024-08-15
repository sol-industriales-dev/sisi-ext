using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Capacitacion
{
    public enum TipoSeguimientoEnum
    {
        [DescriptionAttribute("Acciones del Ciclo de Trabajo")]
        acciones = 1,
        [DescriptionAttribute("Propuestas de Personal Operativo")]
        propuestas = 2,
    }
}
