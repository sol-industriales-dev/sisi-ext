using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum TipoFacultamientoEnum
    {
        [DescriptionAttribute("NO ASIGNADO")]
        NO_ASIGNADO = 0,
        [DescriptionAttribute("ADMINISTRADOR PMO")]
        ADMINISTRADOR_PMO = 1,
        [DescriptionAttribute("ADMINISTRADOR")]
        ADMINISTRADOR = 2,
        [DescriptionAttribute("EVALUADOR")]
        EVALUADOR = 3,
        [DescriptionAttribute("CONSULTA")]
        CONSULTA = 4
    }
}
