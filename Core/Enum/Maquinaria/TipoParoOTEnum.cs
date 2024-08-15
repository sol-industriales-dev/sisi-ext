using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum TipoParoOTEnum
    {
        [DescriptionAttribute("P")]
        Programado = 1,
        [DescriptionAttribute("NP")]
        NoProgramado = 2
    }
}
