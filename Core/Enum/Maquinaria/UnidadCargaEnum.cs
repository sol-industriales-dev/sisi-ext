using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum UnidadCargaEnum
    {
        [DescriptionAttribute("No Aplica")]
        NA = 0,
        [DescriptionAttribute("Toneladas")]
        Toneladas = 1,
        [DescriptionAttribute("Litros")]
        GASOLINA = 2
    }
}
