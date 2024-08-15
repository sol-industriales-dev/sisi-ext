using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Barrenacion
{
    public enum TipoOperadorEnum
    {
        [DescriptionAttribute("Operador")]
        Operador = 1,
        [DescriptionAttribute("Ayudante")]
        Ayudante = 2
    }
}
