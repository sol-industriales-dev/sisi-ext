using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Banco
{
    /// <summary>
    /// Asiga el orden y agrupamiento del registro
    /// </summary>
    public enum formatoRptBanMovEnum
    {
        [DescriptionAttribute("Detalle")]
        Detalle = 1,
        [DescriptionAttribute("Detalle Por CC")]
        DetallePorCc = 2,
        [DescriptionAttribute("Resumen")]
        Resumen = 3,
        [DescriptionAttribute("Resumen Por Cc")]
        ResumenPorCc = 4,
    }
}
