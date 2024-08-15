using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Nomina
{
    public enum TipoDescuentoPrenominaEnum
    {
        [DescriptionAttribute("PRESTAMO")]
        Prestamo = 1,
        [DescriptionAttribute("AXA")]
        Axa = 2,
        [DescriptionAttribute("FAMSA")]
        Famsa = 3,
        [DescriptionAttribute("PENSION ALIMENTICIA")]
        PensionAlimenticia = 4,
        [DescriptionAttribute("FONACOT")]
        Fonacot = 5,
        [DescriptionAttribute("INFONAVIT")]
        Infonavit = 6,
    }
}