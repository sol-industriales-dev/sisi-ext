using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Core.Enum.Maquinaria.Barrenacion
{
    public enum TipoBarrenoEnum
    {
        [DescriptionAttribute("Normal")]
        Normal = 1,
        [DescriptionAttribute("Rehabilitación")]
        Rehabilitacion = 2
    }
}
