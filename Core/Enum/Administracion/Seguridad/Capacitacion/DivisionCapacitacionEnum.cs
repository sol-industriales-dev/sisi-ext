using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum DivisionCapacitacionEnum
    {
        [DescriptionAttribute("Minería")]
        MINERIA = 1,
        [DescriptionAttribute("Construcción Pesada")]
        CONSTRUCCION_PESADA = 2,
        [DescriptionAttribute("Industrial")]
        INDUSTRIAL = 3,
        [DescriptionAttribute("Subterránea")]
        SUBTERRANEA = 4,
        [DescriptionAttribute("Centrales")]
        CENTRALES = 5
    }
}
