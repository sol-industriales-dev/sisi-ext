using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Mantenimiento
{
    public enum MantenimientoEstadoEnum
    {
        [DescriptionAttribute("VIGENTE")]
        VIGENTE = 1,
        [DescriptionAttribute("PROGRAMADO")]
        PROGRAMADO = 2,
        [DescriptionAttribute("EJECUTADO")]
        EJECUTADO = 3,
        [DescriptionAttribute("VENCIDO")]
        VENCIDO = 4
    }
}
